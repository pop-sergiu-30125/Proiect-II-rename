using ProiectII.Models;

namespace ProiectII.Interfaces
{
    public interface IAdoptionRepository : IGenericRepository<Adoption>
    {
<<<<<<< HEAD
        Task<IEnumerable<Adoption>> GetAllWithDetailsAsync();
        Task<Adoption?> GetByIdWithDetailsAsync(uint id);
    }
}
=======
        Task<IEnumerable<Adoption>> GetAdoptionsWithDetailsAsync();
    }
}
>>>>>>> origin/master
