using ColorsApi.Configurations;
using ColorsApi.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ColorsApi;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        builder.Services.AddOpenApi(); // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

        builder.Services.AddDbContext<ColorsDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("ColorsDb"))); 
        
        // Création d'un utilisateur (configuration)
        builder.Services
            .AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>();

        builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            options
                .UseNpgsql(
                    // Même base mais schéma différent
                    builder.Configuration.GetConnectionString("ColorsDb"),
                    npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName)));

        builder.ConfigureTelemetry();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        
        app.Run();
    }
}