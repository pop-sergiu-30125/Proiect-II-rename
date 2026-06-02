using Microsoft.AspNetCore.Mvc;

namespace ProiectII.Controllers
{
    [Route("Reviews")]
    public class ReviewsViewController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/Reviews/Index.cshtml");
        }
    }
}