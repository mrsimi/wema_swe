using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.RegularExpressions;
using wema_swe.Data;
using wema_swe.DTO.Requests;
using wema_swe.DTO.Responses;
using wema_swe.Helpers;
using wema_swe.Interfaces;
using wema_swe.Models;

namespace wema_swe.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IOtpRepository _otpRepository;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private string AppFileBasePath = string.Empty;
        public CustomerRepository(AppDbContext appDbContext, IOtpRepository otpRepository,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _appDbContext = appDbContext;   
            _otpRepository = otpRepository;

            _environment = environment;

            AppFileBasePath = _environment.ContentRootPath; 
        }

        public async Task<GenericResponse<CompleteOnboardingResponse>> CompleteCustomerOnboarding(OnboardingCompletionRequest request)
        {
            var customer = await _appDbContext.Customers.FirstOrDefaultAsync(m => m.PhoneNumber.Equals(request.PhoneNumber));
            if (customer == null)
            {
                return new GenericResponse<CompleteOnboardingResponse>()
                {
                    HttpStatusCode = (int)HttpStatusCode.NotFound,
                    ResponseMessage = "Customer with phonenumber does not exist"
                };
            }


            var otpResponse = await _otpRepository.ConfirmOtp(request);

            if(otpResponse.HttpStatusCode != (int)HttpStatusCode.OK)
            {
                return new GenericResponse<CompleteOnboardingResponse>()
                {
                    HttpStatusCode = otpResponse.HttpStatusCode,
                    ResponseMessage = otpResponse.ResponseMessage
                };
            }

            customer.IsVerified = true;

            _appDbContext.SaveChanges();

            return new GenericResponse<CompleteOnboardingResponse>()
            {
                Data = new CompleteOnboardingResponse
                {
                    Email = customer.Email, 
                    PhoneNumber = customer.PhoneNumber, 
                    Lga = customer.Lga, 
                    State = customer.StateOfResidence
                },
                HttpStatusCode = (int)HttpStatusCode.OK,
                ResponseMessage = "Onboarding Successfully completed"
            };
        }

        public async Task<GenericResponse<List<CustomerResponse>>> GetOnboardedCustomers()
        {
            var onboardedCustomers = await _appDbContext.Customers.Where(m => m.IsVerified)
                    .Select(m => new CustomerResponse { Id = m.Id, Email = m.Email, PhoneNumber = m.PhoneNumber, Lga = m.Lga, StateOfResidence = m.StateOfResidence }).ToListAsync();

            return new GenericResponse<List<CustomerResponse>>
            {
                Data = onboardedCustomers,
                HttpStatusCode = (int)HttpStatusCode.OK,
                ResponseMessage = "Data Request Successful"
            };
        }

        public async Task<GenericResponse<OnboardingInitiationResponse>> InitiateCustomerOnboarding(OnboardingInitiationRequest request)
        {
            //is user exist before
            var customer = await _appDbContext.Customers.FirstOrDefaultAsync(m => m.Email.ToLower().Equals(request.Email.ToLower()) ||
                         m.PhoneNumber.ToLower().Equals(request.PhoneNumber.ToLower()));
            if (customer != null)
            {
                if(customer.IsVerified)
                {
                    return new GenericResponse<OnboardingInitiationResponse>
                    {
                        Data = null, 
                        HttpStatusCode = (int)HttpStatusCode.Conflict, 
                        ResponseMessage = "User with email or phonenumber already exists"
                    };
                }

                else
                {
                    var otpResponse = await _otpRepository.SendOtp(customer.PhoneNumber);
                    if(otpResponse.HttpStatusCode != (int)HttpStatusCode.OK)
                    {
                        return new GenericResponse<OnboardingInitiationResponse>
                        {
                            Data = null, 
                            HttpStatusCode = otpResponse.HttpStatusCode,
                            ResponseMessage = "Customer already Exists, Error occured while trying to send an Otp to phonenumber"
                        };
                    }
                    else
                    {
                        return new GenericResponse<OnboardingInitiationResponse>
                        {
                            Data = new OnboardingInitiationResponse
                            {
                                VerificationReference = otpResponse.Data.VerificationReference,
                                Countdown = otpResponse.Data.CountDown
                            },
                            HttpStatusCode = (int)HttpStatusCode.OK,
                            ResponseMessage = $"Otp sent to user with phonenumber {request.PhoneNumber}"
                        };
                    }
                }
            }


            string passwordStrengthMessage = string.Empty;
            if(!SecurityHelper.ValidatePassword(request.Password, out passwordStrengthMessage))
            {
                return new GenericResponse<OnboardingInitiationResponse>
                {
                    Data = null,
                    HttpStatusCode = (int)HttpStatusCode.BadRequest,
                    ResponseMessage = passwordStrengthMessage
                };
            }




            var isLgaPresent = LgaHelper.IsLgaInState(AppFileBasePath, request.Lga, request.StateOfResidence);
            if(!isLgaPresent)
            {
                return new GenericResponse<OnboardingInitiationResponse>
                {
                    Data = null,
                    HttpStatusCode = (int)HttpStatusCode.BadRequest,
                    ResponseMessage = "Lga provided is not in the state entered"
                };
            }


            string passwordSalt = SecurityHelper.GetSalt();
            string passwordHash = SecurityHelper.GetHash(request.Password, passwordSalt);
            var customerData = new Customer
            {
                PhoneNumber = request.PhoneNumber,
                Email = request.Email, 
                StateOfResidence = request.StateOfResidence, 
                Lga = request.Lga, 
                PasswordSalt = passwordSalt, 
                PasswordHash = passwordHash, 
                DateTimeCreated = DateTime.UtcNow
            };


            await _appDbContext.Customers.AddAsync(customerData);
            await _appDbContext.SaveChangesAsync();


            var sendOtpResponse = await _otpRepository.SendOtp(request.PhoneNumber);
            if (sendOtpResponse.HttpStatusCode != (int)HttpStatusCode.OK)
            {
                return new GenericResponse<OnboardingInitiationResponse>
                {
                    Data = null,
                    HttpStatusCode = sendOtpResponse.HttpStatusCode,
                    ResponseMessage = "Customer already Exists, Error occured while trying to send an Otp to phonenumber"
                };
            }

            return new GenericResponse<OnboardingInitiationResponse>
            {
                Data = new OnboardingInitiationResponse
                {
                    VerificationReference = sendOtpResponse.Data.VerificationReference, 
                    Countdown = sendOtpResponse.Data.CountDown
                },
                HttpStatusCode = (int)HttpStatusCode.OK,
                ResponseMessage = $"Otp sent to user with phonenumber {request.PhoneNumber}"
            };

        }
    }
}
