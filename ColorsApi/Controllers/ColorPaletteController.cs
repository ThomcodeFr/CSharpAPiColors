using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ColorsApi.Dtos;
using ColorsApi.DataBase; 
using ColorsApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ColorsApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ColorPaletteController : ControllerBase
{
    private readonly ColorsDbContext _context;

    public ColorPaletteController(ColorsDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetColors()
    {
        var colorPalettes = await _context.ColorPalettes
            .Where(cp => !cp.IsArchived)
            .Include(cp => cp.Colors.Where(c => !c.IsArchived))
            .ToListAsync();

        var colorPaletteDtos = colorPalettes.Select(cp => new ColorPaletteDto(
            cp.Colors.Select(c => ColorDto.FromColor(c)).ToList()
        )).ToList();

        return Ok(new { Items = colorPaletteDtos });
    }

    [HttpPost]
    public async Task<IActionResult> PostColors([FromBody] ColorPaletteDto colorPaletteDto)
    {
        if (colorPaletteDto == null || !colorPaletteDto.Colors.Any())
        {
            return BadRequest("La palette doit contenir au minimum une couleur");
        }
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Utilisateur non authentifié");
        }
        
        var colorPalette = new ColorPalette
        {
            ColorUserEntityId = userId,
            Colors = colorPaletteDto.Colors.Select(c => new Color
            {
                ColorPaletteId = Guid.NewGuid().ToString(),
                Type = c.Type,
                Red = c.Red,
                Green = c.Green,
                Blue = c.Blue
            }).ToList()
        };

        _context.ColorPalettes.Add(colorPalette);
        await _context.SaveChangesAsync();
        
        foreach (var color in colorPalette.Colors)
        {
            color.ColorPaletteId = colorPalette.Id;
        }
        await _context.SaveChangesAsync();

        var createdDto = new ColorPaletteDto(
            colorPalette.Colors.Select(c => ColorDto.FromColor(c)).ToList()
        );

        return CreatedAtAction(nameof(GetColors), new { id = colorPalette.Id }, new { Items = new[] { createdDto } });
    }

    [HttpDelete("colors/{id}")]
    public async Task<IActionResult> DeleteColor(string id)
    {
        var color = await _context.Colors
            .FirstOrDefaultAsync(c => c.Id == id);

        if (color == null)
        {
            Console.WriteLine($"Color with ID {id} not found");
            return NotFound();
        }

        color.IsArchived = true;
        color.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }}