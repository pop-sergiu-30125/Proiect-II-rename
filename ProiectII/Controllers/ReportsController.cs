using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProiectII.DTO.CommentsReport;
using ProiectII.Interfaces;
using System.Security.Claims;

namespace ProiectII.Controllers
{
    // Acest controller gestionează atât paginile (Views), cât și API-ul pentru Rapoarte
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // ==========================================
        // 1. RUTE PENTRU PAGINI (VIEWS)
        // ==========================================

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Securitate pentru formulare MVC
        public async Task<IActionResult> Create(CreateReportDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            string? currentUserId = null;
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            try
            {
                await _reportService.CreateReportAsync(dto, currentUserId);
                TempData["SuccessMessage"] = "Raportul a fost trimis cu succes!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Eroare la trimiterea raportului: " + ex.Message);
                return View(dto);
            }
        }

        // ==========================================
        // 2. RUTE PENTRU API (JSON)
        // ==========================================
        // Am păstrat prefixul /api/ pentru compatibilitate cu Swagger și JS

        [HttpGet("/api/Reports")]
        public async Task<IActionResult> GetAllApi()
        {
            var reports = await _reportService.GetAllActiveReportsAsync();

            bool isStaff = User.IsInRole("Admin") || User.IsInRole("Employee");

            if (isStaff)
            {
                return Ok(reports);
            }
            var currentUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(currentUserEmail))
            {
                return Unauthorized("Identitate nedetectată.");
            }

            var personalReports = reports.Where(r => r.ReporterName == currentUserEmail);
            return Ok(personalReports);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpPut("/api/Reports/{id}/status")]
        public async Task<IActionResult> UpdateStatus(uint id, [FromBody] UpdateReportStatusDto dto)
        {
            var success = await _reportService.UpdateReportStatusAsync(id, dto);
            if (!success) return BadRequest("Status invalid sau raport negăsit.");

            return Ok(new { Message = "Statusul raportului a fost actualizat." });
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpDelete("/api/Reports/{id}")]
        public async Task<IActionResult> Delete(uint id)
        {
            var success = await _reportService.DeleteReportAsync(id);
            if (!success) return NotFound();

            return Ok(new { Message = "Raport șters permanent (inclusiv imaginea)." });
        }

        [HttpPost("/api/Reports/create")]
        public async Task<IActionResult> CreateReportApi([FromForm] CreateReportDto dto)
        {
            string? currentUserId = null;

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            var createdReport = await _reportService.CreateReportAsync(dto, currentUserId);
            return Ok(createdReport);
        }
    }
}
