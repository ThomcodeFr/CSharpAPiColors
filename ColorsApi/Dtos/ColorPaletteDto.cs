using Enum = ColorsApi.Helpers.Enum;

namespace ColorsApi.Dtos;

internal record ColorPaletteDto(List<ColorDto> Colors)
{
    public static ColorPaletteDto RandomPalette()
    {
        var random = new Random();
        var colors = new List<ColorDto>
        {
            new ColorDto(Enum.ColorType.Primary, random.Next(256), random.Next(256), random.Next(256)),
            new ColorDto(Enum.ColorType.Secondary, random.Next(256), random.Next(256), random.Next(256)),
            new ColorDto(Enum.ColorType.Tertiary, random.Next(256), random.Next(256), random.Next(256)),
            new ColorDto(Enum.ColorType.Accent, random.Next(256), random.Next(256), random.Next(256)),
            new ColorDto(Enum.ColorType.Neutral, random.Next(256), random.Next(256), random.Next(256))
        };
        return new ColorPaletteDto(colors);
    }
}