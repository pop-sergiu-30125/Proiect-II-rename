namespace ProiectII.DTO.FoxManagement
{
    public class UpdateFoxDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public uint StatusId { get; set; }
        public uint? EnclosureId { get; set; }
        
        // Latitude and Longitude are now handled by a separate endpoint, 
        // but keeping them here for compatibility if needed.
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
