using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProiectII.DTO.AuthAccount;
using ProiectII.Interfaces;
using ProiectII.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;

    // SINGURUL constructor permis - aici mufăm toate dependențele
    public AuthController(
        IAuthService authService,
        UserManager<ApplicationUser> userManager,
        IEmailService emailService)
    {
        _authService = authService;
        _userManager = userManager;
        _emailService = emailService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var authResponse = await _authService.LoginAsync(dto);
        if (authResponse == null) return Unauthorized(new { Message = "Date incorecte." });

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,           // OBLIGATORIU avem HTTPS
            SameSite = SameSiteMode.None, // Necesar pentru Cross-Origin între porturi diferite
            Expires = DateTime.UtcNow.AddDays(7),
            Path = "/"
        };
        Response.Cookies.Append("jwt_access_token", authResponse.Token, cookieOptions);

        return Ok(new { authResponse.UserRole, Message = "Logat cu succes." });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto, "User");
        if (!result.IsSuccess) return BadRequest(new { Message = result.Message });
        return Ok(new { Message = result.Message });
    }

   
    [HttpGet("check-auth")]
    [AllowAnonymous]
    public IActionResult CheckAuth()
    {
        return Ok(new
        {
            IsAuthenticated = true,
            UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
            Role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt_access_token", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/"
        });
        return Ok(new { Message = "Delogat." });
    }


    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            return Ok(new { message = "If the email exists, instructions have been sent." });

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var encodedToken = Uri.EscapeDataString(token);
        var encodedEmail = Uri.EscapeDataString(user.Email);

        // Folosim portul 8443 care este cel expus de Nginx/Proxy în docker-compose.yml
        var resetLink =
            $"https://localhost:8443/AuthResetView/ResetPassword?email={encodedEmail}&token={encodedToken}";

        Console.WriteLine("===== PASSWORD RESET DEBUG =====");
        Console.WriteLine($"Email: {user.Email}");
        Console.WriteLine($"Token (RAW): {token}");
        Console.WriteLine($"Encoded Token: {encodedToken}");
        Console.WriteLine($"Reset Link: {resetLink}");
        Console.WriteLine("================================");

        var htmlBody = $@"
<div style='font-family:Arial;padding:20px'>
    <h2>Password Reset</h2>

    <p>Hello,</p>

    <p>You requested a password reset for your account.</p>

    <p>
        Click the button below to reset your password:
    </p>

    <a href='{resetLink}'
       style='display:inline-block;padding:10px 20px;background:#007bff;color:white;text-decoration:none;border-radius:5px;'>
        Reset Password
    </a>

    <hr style='margin-top:20px' />

    <p style='color:gray;font-size:12px;'>
        If you did not request this action, you can safely ignore this email.
    </p>
</div>
";

        await _emailService.SendEmailAsync(
            user.Email,
            "Password Reset",
            htmlBody
        );

        return Ok(new { message = "If the email exists, instructions have been sent." });
    }



    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null) return BadRequest(new { message = "Cerere invalidă." });

        var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
        
        if (!result.Succeeded)
        {
            return BadRequest(new { message = "Eroare la resetare.", errors = result.Errors.Select(e => e.Description) });
        }

        return Ok(new { message = "Parola a fost actualizată." });
    }
}