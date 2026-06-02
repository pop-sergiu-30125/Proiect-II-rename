using System.ComponentModel.DataAnnotations;

namespace ProiectII.DTO.Reviews
{
    public class CreateAppReviewDto
    {
        [Range(1, 5, ErrorMessage = "the mark must be between 1 and 5.")]
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
