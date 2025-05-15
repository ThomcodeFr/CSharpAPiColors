using ColorsApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Testcontainers.PostgreSql;

namespace ColorsApiTests;

/// <summary>
/// Prend le programme et démarre un faux programme pour les tests. Tests d'intégration.
/// </summary>
public class ColorsApiWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreContainer = new PostgreSqlBuilder()
        .WithImage("postgres:17.4")
        .WithDatabase("colorsapi")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();
    
    // Redirection vers une base simulé
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:ColorsDb", _postgreContainer.GetConnectionString());
    }
        
    public async Task InitializeAsync()
    {
        await _postgreContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgreContainer.StartAsync();
    }
}
/// <summary>
/// Represents a base test fixture for integration tests utilizing a custom
/// </summary>
public abstract class IntegrationTestFixture(ColorsApiWebAppFactory factory) : IClassFixture<ColorsApiWebAppFactory>
{
    public HttpClient CreateClient() => factory.CreateClient();
}