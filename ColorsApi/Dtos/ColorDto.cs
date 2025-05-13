using ColorsApi.Entities;

namespace ColorsApi.Dtos;

public class ColorDto
{
    public required string Id { get; set; }
    public required string Type { get; set; }
    public int Red { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }

    public static ColorDto FromColor(Color color)
    {
        return new ColorDto
        {
            Id = color.Id,
            Type = color.Type,
            Red = color.Red,
            Green = color.Green,
            Blue = color.Blue
        };
    }
}