namespace wema_swe.Models
{
    public class Otp
    {
        public int Id { get; set; }
        public string VerificationReference { get; set; }
        public string OtpHash { get; set; }
        public string OtpSalt { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
    }
}
