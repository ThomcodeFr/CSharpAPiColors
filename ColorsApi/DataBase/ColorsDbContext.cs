using ColorsApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ColorsApi.DataBase;

/// <summary>
/// Gère les tables de l'application (Users, ColorPalettes, Colors). 
/// </summary>
public class ColorsDbContext : DbContext
{
    public DbSet<ColorUserEntity> Users { get; set; }
    public DbSet<ColorPalette> ColorPalettes { get; set; }
    public DbSet<Color> Colors { get; set; }

    public ColorsDbContext(DbContextOptions<ColorsDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ColorPalette>()
            .HasMany(cp => cp.Colors)
            .WithOne(c => c.ColorPalette)
            .HasForeignKey(c => c.ColorPaletteId);
        
        modelBuilder.Entity<ColorUserEntity>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasOne(u => u.IdentityUser)
                .WithOne()
                .HasForeignKey<ColorUserEntity>(u => u.IdentityId);
            entity.HasMany(u => u.ColorPalettes)
                .WithOne(p => p.ColorUserEntity)
                .HasForeignKey(p => p.ColorUserEntityId);
        });
    }
}