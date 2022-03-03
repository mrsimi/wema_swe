
using System.Text.Json;
using wema_swe.ThirdParty.DTO.Responses;

namespace wema_swe.Helpers
{
    public static class LgaHelper
    {
        public static bool IsLgaInState(string baseFilePath, string lga, string state)
        {
            bool isLgaPresent = false; 
            var jsonString = File.ReadAllText($"{baseFilePath}/AppFiles/lgas.json");
            var jsonModel = JsonSerializer.Deserialize<List<LgaResponse>>(jsonString);


            var lgas = jsonModel.FirstOrDefault(m => m.state.Equals(state, StringComparison.InvariantCultureIgnoreCase));


            if(lgas != null)
            {
                isLgaPresent = lgas.lgas.Any(m => m.Equals(lga, StringComparison.CurrentCultureIgnoreCase));

            }


            return isLgaPresent;
        }
    }
}
