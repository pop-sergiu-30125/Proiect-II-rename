using ProiectII.Models;

namespace ProiectII.ViewModels
{
    public class UserManagementViewModel
    {
        public List<UserWithRolesViewModel> Users { get; set; } = new List<UserWithRolesViewModel>();
    }

    public class UserWithRolesViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime LastLogin { get; set; }
    }
}