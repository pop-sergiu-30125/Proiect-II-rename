namespace ProiectII.DTO.AdminSystem
{
    public class UserManagementDto
    {
        public List<UserWithRolesDto> Users { get; set; } = new List<UserWithRolesDto>();
    }

    public class UserWithRolesDto
    {
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
