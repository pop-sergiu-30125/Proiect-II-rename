using Microsoft.EntityFrameworkCore;
using ProiectII.Data;
using ProiectII.Interfaces;
using ProiectII.Models;

namespace ProiectII.Repositories
{
    public class AppReviewRepository : GenericRepository<AppReview>, IAppReviewRepository
    {
        public AppReviewRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AppReview>> GetVisibleAsync()
        {
            return await _dbSet
                .Include(r => r.User)
                .Where(r => !r.IsHidden)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public override async Task<AppReview?> GetByIdAsync(uint id)
        {
            return await _dbSet
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(AppReview review)
        {
            await _dbSet.AddAsync(review);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AppReview>> GetAllAdminAsync()
        {
            return await _dbSet
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public IQueryable<AppReview> GetVisibleQuery()
        {
            return _dbSet.Where(r => !r.IsHidden);
        }
    }
}