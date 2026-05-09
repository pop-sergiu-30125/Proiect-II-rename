<<<<<<< HEAD
﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProiectII.DTO.FoxManagement;
using ProiectII.Interfaces;
using ProiectII.Services.CoreDomain;

namespace ProiectII.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FoxController(IFoxService foxService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetFoxes() => Ok(await foxService.GetAllFoxesAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFox(uint id)
    {
        var result = await foxService.GetFoxByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [Authorize(Roles = "Admin,Employee")]
    [HttpPost]
    public async Task<IActionResult> CreateFox([FromForm] CreateFoxDto dto)
    {
        var result = await foxService.CreateFoxAsync(dto);
        return Ok(new { Message = "Vulpe creată cu succes", Data = result });
    }

    [Authorize(Roles = "Admin,Employee")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFox(uint id, [FromBody] UpdateFoxDto dto)
    {
        var success = await foxService.UpdateFoxAsync(id, dto);
        return success ? Ok(new { Message = "Date actualizate." }) : NotFound();
    }

    [Authorize(Roles = "Admin,Employee")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateFoxStatus(uint id, [FromBody] UpdateFoxStatusDto dto)
    {
        var success = await foxService.UpdateFoxStatusAsync(id, dto);
        return success ? Ok(new { Message = "Status actualizat." }) : NotFound();
    }

    [Authorize(Roles = "Admin,Employee")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> ArchiveFox(uint id)
    {
        var success = await foxService.ArchiveFoxAsync(id);
        return success ? Ok(new { Message = "Vulpe arhivată." }) : NotFound();
    }






    [AllowAnonymous]
    [HttpGet("map-markers")]
    public async Task<IActionResult> GetMapMarkers()
    {
        var activeMarkers = await foxService.GetMapMarkersAsync();

        return Ok(activeMarkers);
    }


=======
﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectII.Data; // Asigură-te că ai namespace-ul corect pentru DbContext
using ProiectII.Models;

namespace ProiectII.Controllers
{
    [ApiController] // Definește clasa ca API
    [Route("api/[controller]")] // Ruta va fi: api/fox
    public class FoxController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FoxController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/fox
        // Aceasta este metoda pe care o va apela colegul tău pentru hartă
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fox>>> GetFoxes()
        {
            return await _context.Foxes.ToListAsync();
        }

        // POST: api/fox
        // Folosește asta în Swagger/Postman pentru a adăuga date
        [HttpPost]
        public async Task<ActionResult<Fox>> PostFox(Fox fox)
        {
            _context.Foxes.Add(fox);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFoxes), new { id = fox.Id }, fox);
        }
    }
>>>>>>> origin/master
}