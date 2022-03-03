using wema_swe.DTO.Responses;

namespace wema_swe.Interfaces
{
    public interface IBankRepository
    {
        Task<GenericResponse<List<BankResponse>>> GetBanks();
    }
}
