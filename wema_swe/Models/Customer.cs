namespace wema_swe.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string StateOfResidence { get; set; }
        public string Lga { get; set; }
        public bool IsVerified { get; set; }
        public string  PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }
}
