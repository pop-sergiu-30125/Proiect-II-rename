using Microsoft.EntityFrameworkCore;
using ProiectII.Data;
using ProiectII.Models;
using ProiectII.Interfaces;
<<<<<<< HEAD

=======
>>>>>>> origin/master
namespace ProiectII.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
<<<<<<< HEAD
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
=======

        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
>>>>>>> origin/master
        }

        public async Task<IEnumerable<Comment>> GetCommentsByFoxIdAsync(uint foxId)
        {
            return await _dbSet
<<<<<<< HEAD
                .AsNoTracking() // Mutat sus, e mai curat
                .Include(c => c.User)
                .Where(c => c.FoxId == foxId && !c.IsDeleted && c.IsVisible)
=======
                .Include(c => c.User) // 
                .Where(c => c.FoxId == foxId && !c.IsDeleted && c.IsVisible) // Soft Delete NU FACEM DELETE, ci doar marcam 
                .AsNoTracking() // ceva ce chat spune ca trebuie pentru performanta
>>>>>>> origin/master
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(string userId)
        {
            return await _dbSet
<<<<<<< HEAD
                .AsNoTracking()
                .Where(c => c.UserId == userId && !c.IsDeleted && c.IsVisible)
                .ToListAsync();
        }

        // CRITIC: Trebuie override pentru a inlocui metoda din GenericRepository
        public override async Task<Comment?> GetByIdAsync(uint id)
=======
                .Where(c => c.UserId == userId && !c.IsDeleted && c.IsVisible)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<Comment?> GetByIdAsync(uint id)
>>>>>>> origin/master
        {
            return await _dbSet
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }
<<<<<<< HEAD
    }
}
=======


        

        public async Task UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }


    }





    }
>>>>>>> origin/master
