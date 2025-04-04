using System.Drawing;
using Enum = ColorsApi.Helpers.Enum;

namespace ColorsApi.Dtos;

public record ColorDto(Enum.ColorType Type, int Red, int Green, int Blue)
{
    public static ColorDto FromColor(Enum.ColorType type, Color color)
    {
        return new ColorDto(type, color.R, color.G, color.B);
    }
}
