using System.ComponentModel.DataAnnotations;

namespace wema_swe.DTO.Requests
{
    public class OnboardingInitiationRequest
    {
        [Required]
        [MinLength(11)]
        public string PhoneNumber { get; set; }
        
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        public string StateOfResidence { get; set; }

        [Required]
        public string Lga { get; set; }
    }
}
