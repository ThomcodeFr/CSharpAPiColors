using System.ComponentModel.DataAnnotations.Schema;

namespace ColorsApi.Entities;

/// <summary>
/// Représente une palette de couleurs, liée à un utilisateur et contenant des couleurs.
/// </summary>
public class ColorPalette
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = null!;

    public required string ColorUserEntityId { get; set; }
    [ForeignKey("ColorUserEntityId")]
    public ColorUserEntity? ColorUserEntity { get; set; }

    public List<Color> Colors { get; set; } = new();

    public bool IsArchived { get; set; } = false;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
}