namespace ProiectII.DTO.CommentsReport
{
    public class CreateReportDto
    {
        public string Description { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public uint? FoxId { get; set; } 
        public IFormFile? ImageFile { get; set; }
    }
}
