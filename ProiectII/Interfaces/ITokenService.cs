<<<<<<< HEAD
﻿using ProiectII.Models;
=======
using ProiectII.Models;
>>>>>>> origin/master

namespace ProiectII.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user, IList<string> roles);
    }
}
