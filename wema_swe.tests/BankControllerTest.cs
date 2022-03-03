using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wema_swe.Controllers;
using wema_swe.DTO.Responses;
using wema_swe.Interfaces;
using Xunit;

namespace wema_swe.tests
{
    
    public class BankControllerTest
    {
        [Fact]
        public async Task BankControllerTest_ReturnsOkResult()
        {
            var fakeBankRepo = A.Fake<IBankRepository>();
            var fakeResponse = A.CollectionOfDummy<BankResponse>(10);
            var fakeGenericResponse = new GenericResponse<List<BankResponse>>
            {
                Data = fakeResponse.ToList(), 
                HttpStatusCode = 200,
            };

            A.CallTo(() => fakeBankRepo.GetBanks())
                .Returns(Task.FromResult(fakeGenericResponse));

            var controller = new BankController(fakeBankRepo);



            var actionResult = await controller.GetBanks() as ObjectResult;

            var bankResponse = actionResult?.Value as GenericResponse<List<BankResponse>>;

            Assert.Equal(200, actionResult?.StatusCode);
            Assert.Equal(fakeResponse.Count(), bankResponse?.Data.Count());
        }

        [Fact]
        public async Task BankControllerTest_ReturnsInternalServerError()
        {
            var fakeBankRepo = A.Fake<IBankRepository>();
            var fakeGenericResponse = new GenericResponse<List<BankResponse>>
            {
                HttpStatusCode = 500,
            };

            A.CallTo(() => fakeBankRepo.GetBanks())
                .Returns(Task.FromResult(fakeGenericResponse));

            var controller = new BankController(fakeBankRepo);



            var actionResult = await controller.GetBanks() as ObjectResult;

            var bankResponse = actionResult?.Value as GenericResponse<List<BankResponse>>;

            Assert.Equal(500, actionResult?.StatusCode);
            Assert.Null(bankResponse?.Data);
        }
    }
}
