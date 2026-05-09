using AutoMapper;
using ProiectII.DTO.CommentsReport;
using ProiectII.Interfaces;
using ProiectII.Models;

namespace ProiectII.Services.CoreDomain
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IFileStorageService _fileService;
        private readonly IMapper _mapper;

        public ReportService(
            IReportRepository reportRepository,
            ILocationRepository locationRepository,
            IFileStorageService fileService,
            IMapper mapper)
        {
            _reportRepository = reportRepository;
            _locationRepository = locationRepository;
            _fileService = fileService;
            _mapper = mapper;
        }

<<<<<<< HEAD
        public async Task<ReportDto> CreateReportAsync(CreateReportDto dto, string? userId)
        {
            // 1. Salvarea imaginii pe disc (Fără poză, sistemul crapă mai sus, e ok)
            string imageUrl = await _fileService.SaveFileAsync(dto.ImageFile, "reports");

            try
            {
                // 2. Crearea Locației
                var location = new Location
                {
                    Name = "Report Location", // Sau poți lăsa null dacă nu ai nevoie de nume
                    Coordinate = new Coordinate
                    {
                        Latitude = (decimal)dto.Latitude,
                        Longitude = (decimal)dto.Longitude
                    }
                };

                await _locationRepository.AddAsync(location);
                await _locationRepository.SaveChangesAsync(); // Aici MySQL ne dă înapoi location.Id

                // 3. Crearea Raportului (Mapare curată, apoi suprascriere manuală)
                var report = _mapper.Map<Report>(dto);

                report.ImageUrl = imageUrl;
                report.LocationId = location.Id;
                report.ReporterId = string.IsNullOrWhiteSpace(userId) ? null : userId; // Securitate adăugată
                report.ReportStatus = ReportStatus.Pending;
                report.CreatedAt = DateTime.UtcNow;

                await _reportRepository.AddAsync(report);
                await _reportRepository.SaveChangesAsync(); // Aici raportul este oficial în DB
                
                var completeReport = await _reportRepository.GetByIdWithDetailsAsync(report.Id);

                if (completeReport == null)
                    throw new Exception("Eroare severă: Raportul a fost salvat, dar nu a putut fi citit înapoi.");

                return _mapper.Map<ReportDto>(completeReport);
            }
            catch (Exception)
            {
                _fileService.DeleteFile(imageUrl);
                throw;
            }
        }

        public async Task<IEnumerable<ReportDto>> GetAllActiveReportsAsync()
        {
=======
        //public async Task<ReportDto> CreateReportAsync(CreateReportDto dto, string? userId)
        //{
        //    // 1. Salvarea imaginii pe disc
        //    string imageUrl = await _fileService.SaveFileAsync(dto.ImageFile, "reports");

        //    try
        //    {
        //        // 2. Crearea și salvarea Locației PRIMA DATĂ
        //        var location = new Location
        //        {
        //            Name = "Report Location",
        //            Coordinate = new Coordinate
        //            {
        //                Latitude = (decimal)dto.Latitude,
        //                Longitude = (decimal)dto.Longitude
        //            }
        //        };


        //        await _locationRepository.AddAsync(location);
        //        // CRITIC: Trebuie să salvăm ca să primim location.Id de la MySQL
        //        await _locationRepository.SaveChangesAsync();

        //        // 3. Crearea Raportului
        //        var report = _mapper.Map<Report>(dto);
        //        report.ImageUrl = imageUrl;
        //        report.LocationId = location.Id; // Acum avem un ID real
        //        report.ReporterId = userId;
        //        report.ReportStatus = ReportStatus.Pending;
        //        report.CreatedAt = DateTime.UtcNow;

        //        await _reportRepository.AddAsync(report);
        //        // CRITIC: Salvăm raportul în MySQL
        //        await _reportRepository.SaveChangesAsync();

        //        // Dacă ai o metodă care aduce raportul cu tot cu detalii, o folosești, 
        //        // altfel returnăm direct maparea pe ce am creat
        //        return _mapper.Map<ReportDto>(report);
        //    }
        //    catch (Exception)
        //    {
        //        // Dacă baza de date a crăpat, ștergem poza de pe disc ca să nu ocupe loc degeaba
        //        _fileService.DeleteFile(imageUrl);
        //        throw;
        //    }
        //}


        public async Task<ReportDto> CreateReportAsync(CreateReportDto dto, string? userId)
        {
            // 1. Imaginea
            string imageUrl = await _fileService.SaveFileAsync(dto.ImageFile, "reports");

            // 2. Locația
            var location = new Location
            {
                Name = "Report Location",
                Coordinate = new Coordinate
                 {
                     Latitude = (decimal)dto.Latitude,
                     Longitude = (decimal)dto.Longitude
                 }



            };
            await _locationRepository.AddAsync(location);
            await _locationRepository.SaveChangesAsync(); // Salvăm ca să avem ID-ul locației

            // 3. MAPAREA (Trebuie să fie prima!)
            // Aceasta curăță obiectul, deci tot ce setăm după ea va rămâne în DB
            var report = _mapper.Map<Report>(dto);

            // 4. SETĂRILE MANUALE (Trebuie să fie ultimele!)
            report.ImageUrl = imageUrl;
            report.LocationId = location.Id;
            report.ReporterId = userId;
            report.ReportStatus = ReportStatus.Pending;
            report.CreatedAt = DateTime.UtcNow;

            // 5. Salvarea Raportului
            await _reportRepository.AddAsync(report);
            await _reportRepository.SaveChangesAsync();

            return _mapper.Map<ReportDto>(report);
        }







        public async Task<IEnumerable<ReportDto>> GetAllActiveReportsAsync()
        {
            // Observație: Asigură-te că metoda GetAllActiveReportsWithDetailsAsync există în ReportRepository
>>>>>>> origin/master
            var reports = await _reportRepository.GetAllActiveReportsWithDetailsAsync();
            return _mapper.Map<IEnumerable<ReportDto>>(reports);
        }

        public async Task<ReportDto?> GetReportByIdAsync(uint id)
        {
<<<<<<< HEAD
            var report = await _reportRepository.GetByIdWithDetailsAsync(id);
            return report == null ? null : _mapper.Map<ReportDto>(report);
=======
            // Observație: Asigură-te că metoda GetByIdWithDetailsAsync există în ReportRepository
            var report = await _reportRepository.GetByIdWithDetailsAsync(id);
            if (report == null) return null;

            return _mapper.Map<ReportDto>(report);
>>>>>>> origin/master
        }

        public async Task<bool> UpdateReportStatusAsync(uint reportId, UpdateReportStatusDto dto)
        {
            var report = await _reportRepository.GetByIdAsync(reportId);
            if (report == null) return false;

<<<<<<< HEAD
            // Tratăm posibilitatea ca UI-ul să trimită un status invalid
            if (Enum.TryParse<ReportStatus>(dto.Status, true, out var newStatus))
            {
                report.ReportStatus = newStatus;
=======
            // Convertim string-ul din DTO (ex: "Investigated") în Enum. 
            // Dacă trimiti ID (uint) din frontend, faci cast (ReportStatus)dto.Status
            if (Enum.TryParse<ReportStatus>(dto.Status, true, out var newStatus))
            {
                report.ReportStatus = newStatus;
                // Dacă ai un LastUpdateAt în model, acum e momentul să-l modifici
                // report.LastUpdateAt = DateTime.UtcNow; 
>>>>>>> origin/master

                _reportRepository.Update(report);
                return await _reportRepository.SaveChangesAsync();
            }

<<<<<<< HEAD
            return false;
=======
            return false; // Status invalid trimis de UI
>>>>>>> origin/master
        }

        public async Task<bool> DeleteReportAsync(uint reportId)
        {
            var report = await _reportRepository.GetByIdAsync(reportId);
            if (report == null) return false;

<<<<<<< HEAD
=======
            // Curățenie industrială: Ștergem și fișierul fizic, nu doar rândul din MySQL
>>>>>>> origin/master
            if (!string.IsNullOrEmpty(report.ImageUrl))
            {
                _fileService.DeleteFile(report.ImageUrl);
            }

            _reportRepository.Delete(report);
            return await _reportRepository.SaveChangesAsync();
        }
    }
}