using Microsoft.EntityFrameworkCore;
using System.Net;
using wema_swe.Data;
using wema_swe.DTO.Requests;
using wema_swe.DTO.Responses;
using wema_swe.Helpers;
using wema_swe.Interfaces;
using wema_swe.Models;

namespace wema_swe.Repositories
{
    public class OtpRepository : IOtpRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration Configuration;
        public OtpRepository(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            Configuration = configuration;
        }
        public async Task<GenericResponse<string>> ConfirmOtp(OnboardingCompletionRequest request)
        {
            
            //var otpData = await _dbContext.Otps.FirstOrDefaultAsync(m => m.PhoneNumber.Equals(phoneNumber, StringComparison.OrdinalIgnoreCase) 
            //    && m.VerificationReference.Equals(verificationReference, StringComparison.OrdinalIgnoreCase));

            //if(otpData == null)
            //{
            //    return new GenericResponse<string>()
            //    {
            //        HttpStatusCode = (int)HttpStatusCode.NotFound,
            //        ResponseMessage = "Invalid Otp"
            //    };
            //}

            //if(otpData.ExpiryDate > DateTime.UtcNow)
            //{
            //    return new GenericResponse<string>()
            //    {
            //        HttpStatusCode = (int)HttpStatusCode.Unauthorized,
            //        ResponseMessage = "Otp has Expired"
            //    };
            //}

            //if (otpData.IsUsed)
            //{
            //    return new GenericResponse<string>()
            //    {
            //        HttpStatusCode = (int)HttpStatusCode.Unauthorized,
            //        ResponseMessage = "Otp has been used previously"
            //    };
            //}

            //bool IsOtpCorrect = SecurityHelper.VerifyHash(Otp, otpData.OtpSalt, otpData.OtpHash);
            //if(!IsOtpCorrect)
            //{
            //    return new GenericResponse<string>
            //    {
            //        HttpStatusCode = (int)HttpStatusCode.Unauthorized,
            //        ResponseMessage = "Invalid Otp"
            //    };
            //}

            //otpData.IsUsed = true;
            
            return new GenericResponse<string>
            {
                HttpStatusCode = (int)HttpStatusCode.OK,
                ResponseMessage = "Otp Verification Successful"
            };


        }

        public async Task<GenericResponse<OtpResponse>> SendOtp(string phoneNumber)
        {
            int countdown = 0; 
            int.TryParse(Configuration.GetSection("OtpSettings:ExpiryTime").Value, out countdown);

            var otp = SecurityHelper.GenerateOtp();
            var otpSalt = SecurityHelper.GetSalt();
            var otpVerificationReference = SecurityHelper.GenerateVerificationReference();
            var otpHash = SecurityHelper.GetHash(otp, otpSalt);


            //thirdparty method to send sms

            //save in database
            //var otpData = new Otp
            //{
            //    OtpHash = otpHash,
            //    OtpSalt = otpSalt,
            //    PhoneNumber = phoneNumber,
            //    ExpiryDate = DateTime.UtcNow.AddMinutes(countdown),
            //    VerificationReference = otpVerificationRef
            //};


            //await _dbContext.Otps.AddAsync(otpData);
            //await _dbContext.SaveChangesAsync();

            return new GenericResponse<OtpResponse>()
            {
                Data = new OtpResponse
                {
                    VerificationReference = otpVerificationReference,
                    CountDown = TimeSpan.FromMinutes(countdown).Minutes,
                },
                HttpStatusCode = (int)HttpStatusCode.OK, 
                ResponseMessage = "Otp successfully sent"
            };

        }
    }
}
