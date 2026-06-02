using ProiectII.DTO;
using ProiectII.DTO.Reviews;

namespace ProiectII.Interfaces
{
    public interface IAppReviewService
    {
        Task<List<AppReviewListItemDto>> GetAllAsync();
        Task CreateAsync(CreateAppReviewDto dto, string? userId);
        Task<bool> HideReviewAsync(uint id, string reason);
        Task<AppReviewPageDto> GetStatsAsync();
    }
}