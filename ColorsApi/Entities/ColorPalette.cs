namespace ColorsApi.Entities;

public class ColorPalette
{
    public int Id { get; set; }
    public List<Color> Colors { get; set; }
    public bool IsArchived { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Constructor by default.
    /// </summary>
    public ColorPalette()
    {
        Colors = new List<Color>();
        IsArchived = false;
    }
}