using wema_swe.DTO.Requests;
using wema_swe.DTO.Responses;

namespace wema_swe.Interfaces
{
    public interface IOtpRepository
    {
        Task<GenericResponse<OtpResponse>> SendOtp(string phoneNumber);
        Task<GenericResponse<string>> ConfirmOtp(OnboardingCompletionRequest request);
    }
}
