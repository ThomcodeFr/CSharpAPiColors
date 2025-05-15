using Enum = ColorsApi.Helpers.Enum;

namespace ColorsApi.Dtos;

/// <summary>
/// Représente les palettes de couleurs.
/// </summary>
/// <param name="Colors"></param>
public class ColorPaletteDto
{
    public List<ColorDto> Colors { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="colors"></param>
    public ColorPaletteDto(List<ColorDto> colors)
    {
        Colors = colors;
    }
}