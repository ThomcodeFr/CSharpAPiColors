using ColorsApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ColorsApi.DataBase;

public class ColorsDbContext : DbContext
{
    public ColorsDbContext(DbContextOptions<ColorsDbContext> options) : base(options)
    {
    }
    
    public DbSet<ColorPalette> ColorPalettes { get; set; }
    public DbSet<Color> Colors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ColorPalette>()
            .HasMany(cp => cp.Colors)
            .WithOne(c => c.ColorPalette)
            .HasForeignKey(c => c.ColorPaletteId);
    }
 }