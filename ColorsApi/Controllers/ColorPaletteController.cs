using Microsoft.AspNetCore.Mvc;
using ColorsApi.Dtos;
using ColorsApi.DataBase; 
using ColorsApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ColorsApi.Controllers;

[ApiController]
[Route("[controller]")] // Route de base : /ColorPalette
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

        var colorPalette = new ColorPalette
        {
            Colors = colorPaletteDto.Colors.Select(c => new Color
            {
                Type = c.Type,
                Red = c.Red,
                Green = c.Green,
                Blue = c.Blue
            }).ToList()
        };

        _context.ColorPalettes.Add(colorPalette);
        await _context.SaveChangesAsync();

        // Utilise FromColor pour inclure les Id dans la réponse
        var createdDto = new ColorPaletteDto(
            colorPalette.Colors.Select(c => ColorDto.FromColor(c)).ToList()
        );

        return CreatedAtAction(nameof(GetColors), new { id = colorPalette.Id }, new { Items = new[] { createdDto } });
    }

    [HttpDelete("colors/{id}")]
    public async Task<IActionResult> DeleteColor(int id)
    {
        var color = await _context.Colors
            .FirstOrDefaultAsync(c => c.Id == id);

        if (color == null)
        {
            Console.WriteLine($"Color with ID {id} not found"); // Log pour débogage
            return NotFound();
        }

        color.IsArchived = true;
        color.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }
}