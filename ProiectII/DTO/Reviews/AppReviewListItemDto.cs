namespace ProiectII.DTO.Reviews
{
    public class AppReviewListItemDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public int Rating { get; set; }
        public string Comment { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
