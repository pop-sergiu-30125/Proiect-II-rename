using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProiectII.DTO.CommentsReport;
using ProiectII.Interfaces;
using ProiectII.Models;
using System.Security.Claims;

namespace ProiectII.Controllers
{
    [Route("Reports")]
    public class ReportsViewController : Controller
    {
        private readonly IReportService _reportService;

        public ReportsViewController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("Create")]
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View("~/Views/Reports/Create.cshtml");
        }

        [HttpPost("Create")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReportDto dto)
        {
            // Dacă modelul nu e valid, returnăm view-ul imediat
            if (!ModelState.IsValid)
            {
                return View("~/Views/Reports/Create.cshtml", dto);
            }

            // Aici apelezi serviciul, exact cum ai cerut
            try
            {
                // Trimitem ce avem la dispoziție, fără să ne batem capul cu DB-ul aici
                await _reportService.CreateReportAsync(dto, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                TempData["SuccessMessage"] = "Sighting report submitted successfully!";
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                // Doar logăm eroarea și returnăm view-ul, fără să încercăm să reparăm baza de date noi
                ModelState.AddModelError("", ex.Message);
                return View("~/Views/Reports/Create.cshtml", dto);
            }
        }

        [HttpGet("Index")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Index(string? search, string? status, string? sort = "newest")
        {
            var reports = await _reportService.GetAllReportsAsync();

            // C# Filtering Logic
            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                reports = reports.Where(r => 
                    (r.Description?.ToLower().Contains(s) ?? false) || 
                    (r.ReporterName?.ToLower().Contains(s) ?? false));
            }

            if (!string.IsNullOrEmpty(status) && status != "all")
            {
                reports = reports.Where(r => r.StatusName?.ToLower() == status.ToLower());
            }

            // C# Sorting Logic
            reports = sort switch
            {
                "oldest" => reports.OrderBy(r => r.CreatedAt),
                _ => reports.OrderByDescending(r => r.CreatedAt)
            };

            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.Sort = sort;

            return View("~/Views/Reports/Index.cshtml", reports);
        }

        [HttpPost("UpdateStatus")]
        [Authorize(Roles = "Admin,Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatusMvc(uint id, ReportStatus newStatus)
        {
            var success = await _reportService.UpdateReportStatusAsync(
                id,
                new UpdateReportStatusDto { Status = newStatus.ToString() }
            );

            if (success)
                TempData["SuccessMessage"] = $"Report status updated to {newStatus}!";
            else
                TempData["ErrorMessage"] = "Failed to update report status.";

            return RedirectToAction(nameof(Index));
        }
    }
}