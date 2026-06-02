using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProiectII.DTO.Reviews;
using ProiectII.Interfaces;
using System.Security.Claims;

namespace ProiectII.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppReviewController : ControllerBase
    {
        private readonly IAppReviewService _service;

        public AppReviewController(IAppReviewService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await _service.GetStatsAsync();
            return Ok(result);
        }

        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppReviewDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _service.CreateAsync(dto, userId);

            return Ok(new { Message = "Review adăugat cu succes." });
        }

      
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/hide")]
        public async Task<IActionResult> Hide(uint id, [FromQuery] string reason)
        {
            var success = await _service.HideReviewAsync(id, reason);

            if (!success)
                return NotFound(new { Message = "Review-ul nu a fost găsit." });

            return Ok(new { Message = "Review ascuns cu succes." });
        }
    }
}