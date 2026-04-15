using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProiectII.Models;

namespace ProiectII.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Map()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
<<<<<<< HEAD

        public IActionResult About()
        {
            return View();
        }
=======
>>>>>>> 2afe62f6ef8578a71c77066d149d195660c74d3a
    }
}
