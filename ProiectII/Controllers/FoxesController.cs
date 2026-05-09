using Microsoft.AspNetCore.Mvc;

namespace ProiectII.Controllers
{
    public class FoxesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}