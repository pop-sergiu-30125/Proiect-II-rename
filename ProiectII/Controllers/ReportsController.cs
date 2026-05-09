using Microsoft.AspNetCore.Authorization;
<<<<<<< HEAD
using Microsoft.AspNetCore.Mvc;
using ProiectII.DTO.CommentsReport;
using ProiectII.Interfaces;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }
    //[HttpPost]
    //[Authorize] // Obligatoriu logat pentru a raporta
    //[Consumes("multipart/form-data")]
    //public async Task<IActionResult> Create([FromForm] CreateReportDto dto)
    //{
    //    // Extragem ID-ul real din token-ul de securitate
    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    //    if (string.IsNullOrEmpty(userId))
    //        return Unauthorized("Utilizator invalid.");

    //    try
    //    {
    //        var result = await _reportService.CreateReportAsync(dto, userId);
    //        return Ok(result);
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

    [HttpGet]
    [Authorize] // Adăugăm Authorize pentru a ne asigura că avem un utilizator identificat
    public async Task<IActionResult> GetAll()
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
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(uint id, [FromBody] UpdateReportStatusDto dto)
    {
        var success = await _reportService.UpdateReportStatusAsync(id, dto);
        if (!success) return BadRequest("Status invalid sau raport negăsit.");

        return Ok(new { Message = "Statusul raportului a fost actualizat." });
    }

    [Authorize(Roles = "Admin,Employee")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(uint id)
    {
        var success = await _reportService.DeleteReportAsync(id);
        if (!success) return NotFound();

        return Ok(new { Message = "Raport șters permanent (inclusiv imaginea)." });
    }


    [HttpPost]
    public async Task<IActionResult> CreateReport([FromForm] CreateReportDto dto)
    {
        string? currentUserId = null;

        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        var createdReport = await _reportService.CreateReportAsync(dto, currentUserId);

        return Ok(createdReport);
    }



=======
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
>>>>>>> origin/master
}