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
    public readonly ColorsDbContext _context;

    public ColorPaletteController(ColorsDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetColors()
    {
        var colorPalettes = await _context.ColorPalettes
            .Where(cp => !cp.IsArchived)
            .Include(cp => cp.Colors)
            .ToListAsync();

        var colorPaletteDtos = colorPalettes.Select(cp => new ColorPaletteDto(
            cp.Colors.Select(c => new ColorDto(c.Type, c.Red, c.Green, c.Blue)).ToList()
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

        var createdDto = new ColorPaletteDto(
            colorPalette.Colors.Select(c => new ColorDto(c.Type, c.Red, c.Green, c.Blue)).ToList()
        );
        
        return CreatedAtAction(nameof(GetColors), new { id = colorPalette.Id }, new { Items = new [] { createdDto } });
    }
    
    // Soft delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteColorPalette(int id)
    {
        var palette = await _context.ColorPalettes.FirstOrDefaultAsync(p => p.Id == id);
        if (palette == null)
        {
            return NotFound();
        }
        
        palette.IsArchived = true;
        palette.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();        
    }
}