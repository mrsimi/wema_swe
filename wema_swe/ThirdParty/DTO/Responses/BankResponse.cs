namespace wema_swe.ThirdParty.DTO.Responses
{
    public class Result
    {
        public string bankName { get; set; }
        public string bankCode { get; set; }
    }

    public class BankResponse
    {
        public List<Result> result { get; set; }
        public object errorMessage { get; set; }
        public object errorMessages { get; set; }
        public bool hasError { get; set; }
        public DateTime timeGenerated { get; set; }
    }
}
