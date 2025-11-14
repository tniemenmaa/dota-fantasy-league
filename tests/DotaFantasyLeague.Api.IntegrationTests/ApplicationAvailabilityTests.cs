using System.Net.Http;
using System.Threading.Tasks;
using DotaFantasyLeague.Api.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace DotaFantasyLeague.Api.IntegrationTests;

public class ApplicationAvailabilityTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private readonly IntegrationTestWebApplicationFactory _factory;

    public ApplicationAvailabilityTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task HomePage_ShouldRenderSuccessfully()
    {
        using HttpClient client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true
        });

        using HttpResponseMessage response = await client.GetAsync("/");
        response.EnsureSuccessStatusCode();

        string html = await response.Content.ReadAsStringAsync();
        Assert.Contains("Welcome to the Dota Fantasy League", html);
    }

    [Fact]
    public async Task SwaggerUi_ShouldBeAvailable()
    {
        using HttpClient client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true
        });

        using HttpResponseMessage response = await client.GetAsync("/swagger/index.html");
        response.EnsureSuccessStatusCode();

        string html = await response.Content.ReadAsStringAsync();
        Assert.Contains("Swagger UI", html);
    }
}
