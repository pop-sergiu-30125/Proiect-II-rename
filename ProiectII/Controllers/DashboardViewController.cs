using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectII.Data;
using ProiectII.DTO;

namespace ProiectII.Controllers
{
    [Authorize(Roles = "Admin,Employee,Staff")]
    public class DashboardViewController(ApplicationDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var stats = new DashboardStatsDto
            {
                TotalFoxes = await _context.Foxes.CountAsync(f => !f.IsDeleted),
                PendingAdoptions = await _context.Adoptions.CountAsync(a => (int)a.AdoptionStatus == 1),
                ActiveReports = await _context.Reports.CountAsync(r => (int)r.ReportStatus != 3),
                TotalUsers = await _context.Users.CountAsync()
            };

            stats.RecentActivities = await _context.Reports
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .Take(5)
                .Select(r => new RecentActivityDto
                {
                    Title = "New Report Received",
                    Description = r.Description.Length > 60 ? r.Description.Substring(0, 57) + "..." : r.Description,
                    Date = r.CreatedAt
                })
                .ToListAsync();

            return View(stats);
        }
    }
}
