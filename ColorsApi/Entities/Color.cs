
using System.ComponentModel.DataAnnotations.Schema;
using Enum = ColorsApi.Helpers.Enum;

namespace ColorsApi.Entities;

/// <summary>
/// Représente une couleur dans une palette.
/// </summary>
public class Color
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = null!;

    public required string ColorPaletteId { get; set; }
    [ForeignKey("ColorPaletteId")]
    public ColorPalette? ColorPalette { get; set; }

    public string Type { get; set; } = "RGB";
    public int Red { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }

    public bool IsArchived { get; set; } = false;
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
}