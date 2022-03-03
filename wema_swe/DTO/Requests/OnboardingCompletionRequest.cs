using System.ComponentModel.DataAnnotations;

namespace wema_swe.DTO.Requests
{
    public class OnboardingCompletionRequest
    {
        [Required]
        [StringLength(8)]
        public string VerificationReference { get; set; }
        [Required]
        [StringLength(5)]
        public string Otp { get; set; }

        [Required]
        [MinLength(11)]
        public string PhoneNumber { get; set; }
    }
}
