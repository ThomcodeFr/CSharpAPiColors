
using Enum = ColorsApi.Helpers.Enum;

namespace ColorsApi.Entities;

public class Color
{
    public int Id { get; set; }
    public ColorsApi.Helpers.Enum.ColorType Type { get; set; }
    public int Red { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }
    public bool IsArchived { get; set; } = false;
    public DateTimeOffset? UpdatedAt { get; set; }
    public int ColorPaletteId { get; set; }
    public ColorPalette? ColorPalette { get; set; }
}