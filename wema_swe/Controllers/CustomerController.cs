using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using wema_swe.DTO.Requests;
using wema_swe.DTO.Responses;
using wema_swe.Interfaces;

namespace wema_swe.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpPost("onboarding/initiate")]
        [ProducesResponseType((int)HttpStatusCode.OK, StatusCode = 200, Type = typeof(GenericResponse<OnboardingInitiationResponse>))]
     
        public async Task<IActionResult> InitiateCustomerOnboarding([FromBody]OnboardingInitiationRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _customerRepository.InitiateCustomerOnboarding(request);

            return StatusCode(response.HttpStatusCode, response);
        }


        [HttpPost("onboarding/complete")]
        [ProducesResponseType((int)HttpStatusCode.OK, StatusCode = 200, Type = typeof(GenericResponse<string>))]
       
        public async Task<IActionResult> CompleteCustomerOnboard([FromBody] OnboardingCompletionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _customerRepository.CompleteCustomerOnboarding(request);

            return StatusCode(response.HttpStatusCode, response);
        }

        [HttpGet("onboarded/customers")]
        [ProducesResponseType((int)HttpStatusCode.OK, StatusCode = 200, Type = typeof(GenericResponse<List<CustomerResponse>>))]
       
        public async Task<IActionResult> GetOnboardedCustomers()
        {
            var response = await _customerRepository.GetOnboardedCustomers();

            return StatusCode(response.HttpStatusCode, response);
        }
    }
}
