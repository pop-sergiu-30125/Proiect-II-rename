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

<<<<<<< HEAD
=======
        [Microsoft.AspNetCore.Authorization.Authorize]
>>>>>>> origin/master
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Map()
        {
            return View();
        }

<<<<<<< HEAD
=======
        public IActionResult About()
        {
            return View();
        }

>>>>>>> origin/master
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
