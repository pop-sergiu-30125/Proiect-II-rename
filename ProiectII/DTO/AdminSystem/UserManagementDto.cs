namespace ProiectII.DTO.AdminSystem
{
    public class UserManagementDto
    {
        public List<UserWithRolesDto> Users { get; set; } = new List<UserWithRolesDto>();
    }

    public class UserWithRolesDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string DeactivationReason { get; set; } = "-";
        public DateTime LastLogin { get; set; }
    }
}
