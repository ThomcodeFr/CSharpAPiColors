using System.Drawing;

namespace ColorsApi.Dtos;

internal record ColorDto(ColorType Type, int Red, int Green, int Blue)
{
    public static ColorDto FromColor(ColorType type, Color color)
    {
        return new ColorDto(type, color.R, color.G, color.B);
    }
}

internal enum ColorType
{
    Primary,
    Secondary,
    Tertiary,
    Accent,
    Neutral
}