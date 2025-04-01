namespace ColorsApi.Dtos;

public record ColorPaletteDto(List<ColorDto> Colors)
{
    public static ColorPaletteDto RandomPalette()
    {
        var random = new Random();
        var colors = new List<ColorDto>
        {
            new ColorDto(ColorType.Primary, random.Next(256), random.Next(256), random.Next(256)),
            new ColorDto(ColorType.Secondary, random.Next(256), random.Next(256), random.Next(256)),
            new ColorDto(ColorType.Tertiary, random.Next(256), random.Next(256), random.Next(256)),
            new ColorDto(ColorType.Accent, random.Next(256), random.Next(256), random.Next(256)),
            new ColorDto(ColorType.Neutral, random.Next(256), random.Next(256), random.Next(256))
        };
        return new ColorPaletteDto(colors);
    }
}