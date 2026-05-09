using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProiectII.DTO.CommentsReport;
using ProiectII.Interfaces;
using System.Security.Claims;

namespace ProiectII.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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
    }
}
