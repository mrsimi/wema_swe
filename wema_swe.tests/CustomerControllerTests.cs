using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using wema_swe.Controllers;
using wema_swe.DTO.Requests;
using wema_swe.DTO.Responses;
using wema_swe.Interfaces;
using Xunit;

namespace wema_swe.tests
{
    public class CustomerControllerTests
    {
        [Fact]
        public async Task InitiateCustomerOnboarding_ValidRequest_ReturnsOk()
        {
            //arrange 
            var fakeRequest = new OnboardingInitiationRequest { Email = "simi@gmail.com", Lga = "osogbo", Password = "Abcde1234",
                PhoneNumber = "0810000", StateOfResidence = "osun" };

            var fakeResponse = new GenericResponse<OnboardingInitiationResponse>()
            {
                HttpStatusCode = 200,
                Data = new OnboardingInitiationResponse { Countdown = 30, VerificationReference = "Ws-1222" }
            };
            var fakeCustomerRepo = A.Fake<ICustomerRepository>();
            A.CallTo(() => fakeCustomerRepo.InitiateCustomerOnboarding(fakeRequest)).Returns(Task.FromResult(fakeResponse));

            var controller = new CustomerController(fakeCustomerRepo);



            //act 
            var actionResult = await controller.InitiateCustomerOnboarding(fakeRequest) as ObjectResult;

            //assert
            Assert.Equal(200, actionResult?.StatusCode);

            var onboardingResponse = actionResult?.Value as GenericResponse<OnboardingInitiationResponse>;
            Assert.Equal(30, onboardingResponse?.Data.Countdown);
        }

        [Fact]
        public async Task InitiateCustomerOnboarding_AlreadyExistUser_ReturnsConflict()
        {
            //arrange 
            var fakeRequest = new OnboardingInitiationRequest
            {
                Email = "simi@gmail.com",
                Lga = "osogbo",
                Password = "Abcde1234",
                PhoneNumber = "0810000",
                StateOfResidence = "osun"
            };

            var fakeResponse = new GenericResponse<OnboardingInitiationResponse>()
            {
                HttpStatusCode = (int)HttpStatusCode.Conflict
            };
            var fakeCustomerRepo = A.Fake<ICustomerRepository>();
            A.CallTo(() => fakeCustomerRepo.InitiateCustomerOnboarding(fakeRequest)).Returns(Task.FromResult(fakeResponse));

            var controller = new CustomerController(fakeCustomerRepo);



            //act 
            var actionResult = await controller.InitiateCustomerOnboarding(fakeRequest) as ObjectResult;

            //assert
            Assert.Equal(409, actionResult?.StatusCode);

            var onboardingResponse = actionResult?.Value as GenericResponse<OnboardingInitiationResponse>;
            Assert.Null(onboardingResponse?.Data);
        }

        [Fact]
        public async Task InitiateCustomerOnboarding_LgaNotInState_ReturnsBadRequest()
        {
            //arrange 
            var fakeRequest = new OnboardingInitiationRequest
            {
                Email = "simi@gmail.com",
                Lga = "norman",
                Password = "Abcde1234",
                PhoneNumber = "0810000",
                StateOfResidence = "osun"
            };

            var fakeResponse = new GenericResponse<OnboardingInitiationResponse>()
            {
                HttpStatusCode = (int)HttpStatusCode.BadRequest
            };
            var fakeCustomerRepo = A.Fake<ICustomerRepository>();
            A.CallTo(() => fakeCustomerRepo.InitiateCustomerOnboarding(fakeRequest)).Returns(Task.FromResult(fakeResponse));

            var controller = new CustomerController(fakeCustomerRepo);



            //act 
            var actionResult = await controller.InitiateCustomerOnboarding(fakeRequest) as ObjectResult;

            //assert
            Assert.Equal(400, actionResult?.StatusCode);

            var onboardingResponse = actionResult?.Value as GenericResponse<OnboardingInitiationResponse>;
            Assert.Null(onboardingResponse?.Data);
        }

        [Fact]
        public async Task CompleteOnboarding_ValidRequest_ReturnsOk()
        {
            //arrange 
            var fakeRequest = A.Dummy<OnboardingCompletionRequest>();
            var fakeCompleteOnboardingResponse = A.Dummy<CompleteOnboardingResponse>();

            var fakeResponse = new GenericResponse<CompleteOnboardingResponse>()
            {
                HttpStatusCode = (int)HttpStatusCode.OK,
                Data = fakeCompleteOnboardingResponse
            };
            var fakeCustomerRepo = A.Fake<ICustomerRepository>();
            A.CallTo(() => fakeCustomerRepo.CompleteCustomerOnboarding(fakeRequest)).Returns(Task.FromResult(fakeResponse));

            var controller = new CustomerController(fakeCustomerRepo);



            //act 
            var actionResult = await controller.CompleteCustomerOnboard(fakeRequest) as ObjectResult;

            //assert
            Assert.Equal(200, actionResult?.StatusCode);

            var onboardingResponse = actionResult?.Value as GenericResponse<string>;
        }



        [Fact]
        public async Task CompleteOnboarding_InValidRequest_ReturnsBadRequest()
        {
            //arrange 
            var fakeRequest = A.Dummy<OnboardingCompletionRequest>();
           
            var fakeResponse = new GenericResponse<CompleteOnboardingResponse>()
            {
                HttpStatusCode = (int)HttpStatusCode.BadRequest
            };
            var fakeCustomerRepo = A.Fake<ICustomerRepository>();
            A.CallTo(() => fakeCustomerRepo.CompleteCustomerOnboarding(fakeRequest)).Returns(Task.FromResult(fakeResponse));

            var controller = new CustomerController(fakeCustomerRepo);



            //act 
            var actionResult = await controller.CompleteCustomerOnboard(fakeRequest) as ObjectResult;

            //assert
            Assert.Equal(400, actionResult?.StatusCode);

            var onboardingResponse = actionResult?.Value as GenericResponse<string>;
            Assert.Null(onboardingResponse?.Data);
        }
    }
}