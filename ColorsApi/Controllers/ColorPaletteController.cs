using Microsoft.AspNetCore.Mvc;
using ColorsApi.Dtos;
using System.Collections.Generic;

namespace ColorsApi.Controllers;

[ApiController] 
[Route("[controller]")]
internal class ColorPaletteController : ControllerBase
{
    [HttpGet]
    public IActionResult GetColors()
    {
        var colorPalettes = new List<ColorPaletteDto>
        {
            ColorPaletteDto.RandomPalette(),
            ColorPaletteDto.RandomPalette(),
            ColorPaletteDto.RandomPalette(),
            ColorPaletteDto.RandomPalette(),
        };

        return Ok(new { Items = colorPalettes });    
    }
}