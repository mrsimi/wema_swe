using Newtonsoft.Json;
using System.Net;
using wema_swe.DTO.Responses;
using wema_swe.Interfaces;

namespace wema_swe.Repositories
{
    public class BankRepository : IBankRepository
    {
        private readonly IConfiguration _configuration;
        private string AlatApiPrivateKey;
        private string AlatApiBaseUrl; 
        public BankRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            AlatApiBaseUrl = _configuration.GetSection("AlatWemaAPI:BaseUrl").Value;
            AlatApiPrivateKey = _configuration.GetSection("AlatWemaAPI:TestPrimaryKey").Value;
        }
        public async Task<GenericResponse<List<BankResponse>>> GetBanks()
        {
            string banksUrl = _configuration.GetSection("AlatWemaAPI:BanksUrl").Value;

            string url = string.Concat(AlatApiBaseUrl, banksUrl);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{AlatApiPrivateKey}");

            var response = await client.GetAsync(url);

           

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<ThirdParty.DTO.Responses.BankResponse>(responseContent);


            if(responseJson.result == null)
            {
                return new GenericResponse<List<BankResponse>>
                {
                    Data = null,
                    ResponseMessage = responseJson.errorMessage.ToString(),
                    HttpStatusCode = (int)response.StatusCode
                };
            }

            List<BankResponse> banks = new List<BankResponse>();
            foreach (var bank in responseJson.result)
            {
                banks.Add(new BankResponse
                {
                    BankCode = bank.bankCode,
                    BankName = bank.bankName
                });
            }

            return new GenericResponse<List<BankResponse>>
            {
                Data = banks,
                ResponseMessage = "Data Request Successful",
                HttpStatusCode = (int)HttpStatusCode.OK
            };

        }
    }
}
