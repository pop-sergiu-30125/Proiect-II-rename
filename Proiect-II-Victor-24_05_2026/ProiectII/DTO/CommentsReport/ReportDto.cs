namespace ProiectII.DTO.CommentsReport
{
    public class ReportDto
    {
        public uint Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ImageUrl { get; set; } = string.Empty; 
        public string StatusName { get; set; } = "Pending"; 
        public DateTime CreatedAt { get; set; }
        public string ReporterName { get; set; } = string.Empty;
    }
}
