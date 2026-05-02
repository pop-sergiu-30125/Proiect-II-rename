using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProiectII.DTO.CommentsReport;
using ProiectII.Interfaces;
using ProiectII.Models;
using System.Security.Claims;

namespace ProiectII.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsController(IReportService reportService, UserManager<ApplicationUser> userManager)
        {
            _reportService = reportService;
            _userManager = userManager;
        }

        // GET: /Reports/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Reports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReportDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var userId = _userManager.GetUserId(User);

            try
            {
                await _reportService.CreateReportAsync(dto, userId);
                TempData["SuccessMessage"] = "Raportul a fost trimis cu succes!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Eroare la salvarea raportului: " + ex.Message);
                return View(dto);
            }
        }

        // API-like endpoint still available if needed via URL /Reports/GetAll
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var reports = await _reportService.GetAllActiveReportsAsync();
            return Json(reports);
        }
    }
}