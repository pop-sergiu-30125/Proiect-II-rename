using System.ComponentModel.DataAnnotations;

namespace ProiectII.DTO.AuthAccount
{
    public class UserProfileDto
    {
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Birth date is required.")]
        public DateOnly BirthDate { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public DateTime LastLogin { get; set; }

        public IFormFile? NewProfilePicture { get; set; }
    }
}
