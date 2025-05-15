using System.Text;
using ColorsApi.Configurations;
using ColorsApi.DataBase;
using ColorsApi.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;

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
                    npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, DbConstants.AuthSchema)));
        
        
        // Gestion de l'authentification
        builder.Services.Configure<JwtAuthOptions>(builder.Configuration.GetSection("Jwt"));
        
        var jwtAuthOptions = builder.Configuration.GetSection("Jwt").Get<JwtAuthOptions>();
        
        if (jwtAuthOptions is null)
        {
            throw new InvalidOperationException("Les options JWT sont manquantes ou mal configurées dans appsettings.json.");
        }
        
        // Protection des ressources
        builder.Services
            .AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtAuthOptions.Issuer,
                    ValidAudience = jwtAuthOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthOptions.Key))
                };
            });
        
        builder.Services.AddAuthorization();

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