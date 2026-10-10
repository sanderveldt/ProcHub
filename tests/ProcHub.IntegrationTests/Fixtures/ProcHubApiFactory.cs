using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace ProcHub.IntegrationTests.Fixtures;

public sealed class ProcHubApiFactory 
    : WebApplicationFactory<Program>, 
        IAsyncLifetime
{
    public const string AdminEmail = 
        "admin@integration.test";
    public const string AdminPassword = 
        "Integration32!";

    private readonly PostgreSqlContainer _postgres = 
        new PostgreSqlBuilder("postgres:17-alpine")
            .WithDatabase("prochub_integration")
            .WithUsername("integration")
            .WithPassword("integration-only-password")
            .Build();
        
    public Task InitializeAsync() => 
        _postgres.StartAsync();

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Production");
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(
                new Dictionary<string, string?>
            {
                ["ConnectionStrings:ProcHubDb"] = 
                    _postgres.GetConnectionString(),
                ["InitialAdmin:Email"] = 
                    AdminEmail,
                ["InitialAdmin:Password"] =
                    AdminPassword
            });
        });
    }


    async Task IAsyncLifetime.DisposeAsync()
    {
        await base.DisposeAsync();
        await _postgres.DisposeAsync();
    }

}

