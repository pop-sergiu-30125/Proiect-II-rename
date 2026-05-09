using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectII.DTO.AuthAccount;
using ProiectII.Interfaces;
using ProiectII.Models;
using ProiectII.ViewModels;

namespace ProiectII.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;

        public AccountController(UserManager<ApplicationUser> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            try
            {
                var authResponse = await _authService.LoginAsync(dto);
                if (authResponse == null)
                {
                    ModelState.AddModelError("", "Email sau parolă incorectă.");
                    return View(dto);
                }

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Path = "/"
                };

                Response.Cookies.Append("jwt_access_token", authResponse.Token, cookieOptions);
                return RedirectToAction("Index", "Home");
            }
            catch (UnauthorizedAccessException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "A apărut o eroare neprevăzută.");
                return View(dto);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var result = await _authService.RegisterAsync(dto, "User");
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message);
                return View(dto);
            }

            TempData["SuccessMessage"] = "Cont creat cu succes! Te poți loga acum.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt_access_token", new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Path = "/"
            });
            return RedirectToAction("Login");
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var viewModel = new UserManagementViewModel();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                viewModel.Users.Add(new UserWithRolesViewModel
                {
                    Email = user.Email ?? "",
                    FullName = $"{user.FirstName} {user.LastName}",
                    Roles = string.Join(", ", roles),
                    IsActive = user.IsActive,
                    LastLogin = user.LastLogin
                });
            }

            return View(viewModel);
        }
    }
}