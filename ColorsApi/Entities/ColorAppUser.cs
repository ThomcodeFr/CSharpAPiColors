namespace ColorsApi.Entities;

public class ColorAppUser
{
    public required string Id { get; set; }
    public bool IsArchived { get; set; } = false;
    public DateTimeOffset? UpdatedAt { get; set; }
    public List<ColorPalette> ColorPalettes { get; set; } = new();
    public required string IdentityUserId { get; set; } // Clé étrangère 
    public Microsoft.AspNetCore.Identity.IdentityUser? IdentityUser { get; set; }
}
