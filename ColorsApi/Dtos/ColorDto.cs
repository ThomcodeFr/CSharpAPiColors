using ColorsApi.Entities;
using Enum = ColorsApi.Helpers.Enum;

namespace ColorsApi.Dtos;

public record ColorDto(int Id, Enum.ColorType Type, int Red, int Green, int Blue)
{
    public static ColorDto FromColor(Color color)
    {
        return new ColorDto(color.Id, color.Type, color.Red, color.Green, color.Blue);
    }
}