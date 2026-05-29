using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProiectII.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class AdoptionsViewController : Controller
    {
        [HttpGet]
        public IActionResult Manage()
        {
            return View();
        }
    }
}