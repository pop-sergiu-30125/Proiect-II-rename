using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProiectII.Interfaces;
using ProiectII.DTO.FoxManagement;

namespace ProiectII.Controllers
{
    public class FoxesController : Controller
    {
        private readonly IFoxService _foxService;

        public FoxesController(IFoxService foxService)
        {
            _foxService = foxService;
        }

        public async Task<IActionResult> Index(string? search, string? status, string? sort = "newest")
        {
            var foxes = await _foxService.GetAllFoxesAsync();

            // C# Filtering
            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                foxes = foxes.Where(f => (f.Name?.ToLower().Contains(s) ?? false));
            }

            if (!string.IsNullOrEmpty(status) && status != "all")
            {
                foxes = foxes.Where(f => (f.StatusName?.ToLower().Contains(status.ToLower()) ?? false));
            }

            // C# Sorting
            foxes = sort switch
            {
                "az" => foxes.OrderBy(f => f.Name),
                "za" => foxes.OrderByDescending(f => f.Name),
                "oldest" => foxes.OrderBy(f => f.Id),
                _ => foxes.OrderByDescending(f => f.Id)
            };

            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.Sort = sort;

            return View(foxes);
        }

        [HttpGet("Foxes/Details/{id}")]
        public async Task<IActionResult> Details(uint id)
        {
            var fox = await _foxService.GetFoxByIdAsync(id);
            if (fox == null) return NotFound();
            return View(fox);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult CreateFox()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(uint id)
        {
            var fox = await _foxService.GetFoxByIdAsync(id);
            if (fox == null) return NotFound();
            return View(fox);
        }
    }
}
