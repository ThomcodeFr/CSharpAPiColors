using ColorsApi.Configurations;
using ColorsApi.DataBase;
using Microsoft.EntityFrameworkCore;

namespace ColorsApi;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.ConfigureTelemetry();
        
        builder.Services.AddDbContext<ColorsDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("ColorsDb")));
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        
        app.MapControllers();
        
        app.Run();
    }
}