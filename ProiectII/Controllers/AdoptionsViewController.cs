using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProiectII.Interfaces;
using ProiectII.DTO.AdoptionProcess;

namespace ProiectII.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class AdoptionsViewController(IAdoptionService _adoptionService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Manage(string? search, string? status, string? sort = "newest")
        {
            var adoptions = await _adoptionService.GetAllAdoptionsAsync();

            // C# Filtering
            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                adoptions = adoptions.Where(a => 
                    (a.FoxName?.ToLower().Contains(s) ?? false) || 
                    (a.ApplicantName?.ToLower().Contains(s) ?? false) ||
                    (a.ApplicantEmail?.ToLower().Contains(s) ?? false));
            }

            if (!string.IsNullOrEmpty(status) && status != "all")
            {
                adoptions = adoptions.Where(a => a.Status?.ToLower() == status.ToLower());
            }

            // C# Sorting
            adoptions = sort switch
            {
                "oldest" => adoptions.OrderBy(a => a.SubmittedAt),
                _ => adoptions.OrderByDescending(a => a.SubmittedAt)
            };

            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.Sort = sort;

            return View(adoptions);
        }
    }
}
