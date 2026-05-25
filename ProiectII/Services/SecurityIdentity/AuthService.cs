
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ProiectII.DTO;
using ProiectII.DTO.AuthAccount;
using ProiectII.Interfaces;
using ProiectII.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;




namespace ProiectII.Services.SecurityIdentity
{


    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return null;

            if (!user.IsActive)
                throw new UnauthorizedAccessException($"Cont dezactivat. Motiv: {user.DeactivationReason}");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordValid) return null;

            user.UpdateLastLogin();
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateToken(user, roles);

            return new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddDays(7),
                UserRole = roles.FirstOrDefault() ?? "User",
                RefreshToken = "NOT_IMPLEMENTED_YET"
            };
        }

        public async Task<MessageDto> RegisterAsync(RegisterDto dto, string role = "User")
        {
            var userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null)
                return new MessageDto { IsSuccess = false, Message = "Email-ul este deja folosit." };

            var newUser = new ApplicationUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BornDate = dto.BirthDate,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(newUser, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new MessageDto { IsSuccess = false, Message = $"Eroare la creare: {errors}" };
            }

            if (await _roleManager.RoleExistsAsync(role))
            {
                await _userManager.AddToRoleAsync(newUser, role);
            }

            return new MessageDto { IsSuccess = true, Message = "Cont creat cu succes!" };
        }
    }
    }

