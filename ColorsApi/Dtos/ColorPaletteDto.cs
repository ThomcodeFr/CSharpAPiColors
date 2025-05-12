using Enum = ColorsApi.Helpers.Enum;

namespace ColorsApi.Dtos;

public record ColorPaletteDto(List<ColorDto> Colors)
{
    public static ColorPaletteDto RandomPalette()
    {
        var random = new Random();
        var colors = new List<ColorDto>
        {
            new ColorDto(0, Enum.ColorType.Primary, random.Next(256), random.Next(256), random.Next(256)),
            new ColorDto(0, Enum.ColorType.Secondary, random.Next(256), random.Next(256), random.Next(256)),
            new ColorDto(0, Enum.ColorType.Tertiary, random.Next(256), random.Next(256), random.Next(256)),
            new ColorDto(0, Enum.ColorType.Accent, random.Next(256), random.Next(256), random.Next(256)),
            new ColorDto(0, Enum.ColorType.Neutral, random.Next(256), random.Next(256), random.Next(256))
        };
        return new ColorPaletteDto(colors);
    }
}