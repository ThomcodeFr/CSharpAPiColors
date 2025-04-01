namespace ColorsApi.Models;

public class Color
{
    public int Type { get; set; }
    public int Red { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }
}

public class ColorPalette
{
    public required List<Color> Colors { get; set; }
}

public class ColorPalettes
{
    public required List<ColorPalette> Items { get; set; }
}