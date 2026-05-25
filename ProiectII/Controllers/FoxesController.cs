using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProiectII.Controllers
{
    public class FoxesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Foxes/Details/{id}")]
        public IActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]


        public IActionResult CreateFox()
        {
            return View();
        }



    }
}