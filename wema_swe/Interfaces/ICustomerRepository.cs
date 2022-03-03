using wema_swe.DTO.Requests;
using wema_swe.DTO.Responses;

namespace wema_swe.Interfaces
{
    public interface ICustomerRepository
    {
        Task<GenericResponse<OnboardingInitiationResponse>> InitiateCustomerOnboarding(OnboardingInitiationRequest request);
        Task<GenericResponse<CompleteOnboardingResponse>> CompleteCustomerOnboarding(OnboardingCompletionRequest request);

        Task<GenericResponse<List<CustomerResponse>>> GetOnboardedCustomers();
    }
}
