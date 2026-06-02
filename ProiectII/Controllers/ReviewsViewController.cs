using Microsoft.AspNetCore.Mvc;
using ProiectII.Interfaces;

namespace ProiectII.Controllers
{
    [Route("Reviews")]
    public class ReviewsViewController(IAppReviewService _reviewService) : Controller
    {
        [HttpGet("")]
        public async Task<IActionResult> Index(string sort = "newest")
        {
            var reviews = await _reviewService.GetAllAsync();
            var stats = await _reviewService.GetStatsAsync();

            // C# Sorting logic
            reviews = sort switch
            {
                "oldest" => reviews.OrderBy(r => r.CreatedAt).ToList(),
                "highest" => reviews.OrderByDescending(r => r.Rating).ToList(),
                "lowest" => reviews.OrderBy(r => r.Rating).ToList(),
                _ => reviews.OrderByDescending(r => r.CreatedAt).ToList()
            };
            
            ViewBag.Stats = stats;
            ViewBag.Sort = sort;
            return View("~/Views/Reviews/Index.cshtml", reviews);
        }
    }
}
