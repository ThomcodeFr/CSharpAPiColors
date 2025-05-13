using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ColorsApi.Entities;

/// <summary>
/// Cette entité représente un utilisateur dans ton application, lié à IdentityUser via IdentityId.
/// </summary>
public class ColorUserEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = null!;

    [ForeignKey("IdentityUser")]
    public required string IdentityId { get; set; }
    public IdentityUser? IdentityUser { get; set; }

    public List<ColorPalette> ColorPalettes { get; set; } = new();

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsArchived { get; set; } = false;
}