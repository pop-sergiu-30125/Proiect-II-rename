using Microsoft.AspNetCore.Mvc;

namespace ProiectII.Controllers
{
    public class AuthResetViewController : Controller
    {
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            ViewBag.Email = email;
            ViewBag.Token = token;
            return View();
        }
    }
}