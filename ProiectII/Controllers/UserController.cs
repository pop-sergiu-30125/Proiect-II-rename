<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProiectII.DTO.AuthAccount;
using ProiectII.Interfaces;
using ProiectII.Models;
using System.Security.Claims;

namespace ProiectII.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileStorageService _fileStorageService;

        public UserController(UserManager<ApplicationUser> userManager, IFileStorageService fileStorageService)
        {
            _userManager = userManager;
            _fileStorageService = fileStorageService;
=======
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectII.DTO.AuthAccount;
using ProiectII.Models;

namespace ProiectII.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            return Ok(new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.BornDate,
                user.ProfilePictureUrl,
                user.LastLogin
            });
>>>>>>> origin/master
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
<<<<<<< HEAD
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

=======
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
>>>>>>> origin/master
            if (user == null) return NotFound();

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.BornDate = dto.BirthDate;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

<<<<<<< HEAD
            return Ok(new { Message = "Profil actualizat cu succes." });
        }

        // 2. Schimbare Parolă (În interiorul contului)
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            // ChangePasswordAsync verifică automat dacă parola veche este corectă
            var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "Parola a fost schimbată." });
        }

        // 3. Încărcare Poză de Profil (Folosind fișier binar)
        [HttpPost("upload-picture")]
        public async Task<IActionResult> UploadPicture(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("Niciun fișier selectat.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            try
            {
                // Salvăm fișierul folosind serviciul tău profesional
                string relativePath = await _fileStorageService.SaveFileAsync(file, "profiles");

                // Ștergem poza veche de pe disc (opțional, dar recomandat pentru spațiu)
                if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
                {
                    _fileStorageService.DeleteFile(user.ProfilePictureUrl);
                }

                // Actualizăm calea în baza de date
                user.ProfilePictureUrl = relativePath;
                await _userManager.UpdateAsync(user);

                return Ok(new { Url = relativePath });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetMyProfile()
        {
   
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound("Utilizatorul nu mai există în sistem.");

            var response = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BornDate,
                ProfilePictureUrl = user.ProfilePictureUrl
            };

            return Ok(response);
        }


    }
}
=======
            return Ok(new { Message = "Profil actualizat." });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "Parolă schimbată cu succes." });
        }
    }
}
>>>>>>> origin/master
