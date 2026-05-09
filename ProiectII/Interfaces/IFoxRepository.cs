using ProiectII.Models;

namespace ProiectII.Interfaces
{
    public interface IFoxRepository : IGenericRepository<Fox>
    {
<<<<<<< HEAD
=======
        // Metoda specifică pentru a aduce vulpea cu Status, Locație și Țarc
>>>>>>> origin/master
        Task<IEnumerable<Fox>> GetFoxesWithDetailsAsync();
        Task<Fox?> GetFoxByIdWithDetailsAsync(uint id);
    }
}