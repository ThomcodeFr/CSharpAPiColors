using Microsoft.AspNetCore.Mvc;
using ColorsApi.Dtos;
using System.Collections.Generic;
using ColorsApi.DataBase;
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
    public async Task<IActionResult> GetColor()
    {
        var colorPalettes = await _context.ColorPalettes
            .Include(cp => cp.Colors)
            .ToListAsync();

        var colorPaletteDtos = colorPalettes.Select(cp => new ColorPaletteDto(
            cp.Colors.Select(c => new ColorDto(c.Type, c.Red, c.Green, c.Blue)).ToList()
        )).ToList();

        return Ok(new { Items = colorPaletteDtos });    
    }
}