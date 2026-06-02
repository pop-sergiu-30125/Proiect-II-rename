using ProiectII.Models;

namespace ProiectII.Interfaces
{
    public interface IAppReviewRepository
    {
        Task<IEnumerable<AppReview>> GetVisibleAsync();
        Task<AppReview?> GetByIdAsync(uint id);

        Task AddAsync(AppReview review);
        Task SaveAsync();

        IQueryable<AppReview> GetVisibleQuery();
    }
}