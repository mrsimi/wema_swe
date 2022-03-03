using Microsoft.AspNetCore.Mvc;
using System.Net;
using wema_swe.DTO.Responses;
using wema_swe.Interfaces;

namespace wema_swe.Controllers
{
    [Route("api/banks")]
    [ApiController]
    public class BankController : ControllerBase
    {

        private readonly IBankRepository _bankRepository;
        public BankController(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, StatusCode = 200, Type = typeof(GenericResponse<List<BankResponse>>))]
      

        public async Task<IActionResult> GetBanks()
        {
            var response = await _bankRepository.GetBanks();

            return StatusCode(response.HttpStatusCode, response);
        }
    }
}
