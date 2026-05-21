using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectII.DTO.AuthAccount;
using ProiectII.DTO.AdminSystem;
using ProiectII.Interfaces;
using ProiectII.Models;

namespace ProiectII.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;
        private readonly IFileStorageService _fileStorageService;

        public AccountController(UserManager<ApplicationUser> userManager, IAuthService authService, IFileStorageService fileStorageService)
        {
            _userManager = userManager;
            _authService = authService;
            _fileStorageService = fileStorageService;
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
                    ModelState.AddModelError("", "Invalid email or password.");
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
                ModelState.AddModelError("", "An unexpected error occurred.");
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

            TempData["SuccessMessage"] = "Account created successfully! You can now log in.";
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
            var viewModel = new UserManagementDto();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                viewModel.Users.Add(new UserWithRolesDto
                {
                    // + posibil de implementat id, deoarece pentru updaterole din AdminUserController trebuie un dto cu acel id
                    Email = user.Email ?? "",
                    FullName = $"{user.FirstName} {user.LastName}",
                    Roles = string.Join(", ", roles),
                    IsActive = user.IsActive,
                    LastLogin = user.LastLogin
                });
            }

            return View(viewModel);
        }

        [HttpGet]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var model = new UserProfileDto
            {
                Email = user.Email ?? "",
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BornDate,
                ProfilePictureUrl = user.ProfilePictureUrl,
                LastLogin = user.LastLogin
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Profile(UserProfileDto model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (!ModelState.IsValid)
            {
                model.Email = user.Email ?? "";
                model.ProfilePictureUrl = user.ProfilePictureUrl;
                return View(model);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.BornDate = model.BirthDate;

            if (model.NewProfilePicture != null && model.NewProfilePicture.Length > 0)
            {
                try
                {
                    string relativePath = await _fileStorageService.SaveFileAsync(model.NewProfilePicture, "profiles");
                    if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
                    {
                        _fileStorageService.DeleteFile(user.ProfilePictureUrl);
                    }
                    user.ProfilePictureUrl = relativePath;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving image: " + ex.Message);
                    model.Email = user.Email ?? "";
                    model.ProfilePictureUrl = user.ProfilePictureUrl;
                    return View(model);
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                model.Email = user.Email ?? "";
                model.ProfilePictureUrl = user.ProfilePictureUrl;
                return View(model);
            }

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (dto.NewPassword != dto.ConfirmPassword)
            {
                TempData["ErrorMessage"] = "New password and confirmation do not match.";
                return RedirectToAction("Profile");
            }

            var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                TempData["ErrorMessage"] = string.Join(", ", result.Errors.Select(e => e.Description));
                return RedirectToAction("Profile");
            }

            TempData["SuccessMessage"] = "Password changed successfully!";
            return RedirectToAction("Profile");
        }
    }
}