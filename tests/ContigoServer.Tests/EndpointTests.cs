using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ContigoServer.Tests;

/// <summary>
/// Boots the real server in-process and talks to it over HTTP. This is the
/// shape every student check takes: a stranger runs `dotnet test`, it fails on
/// main, it passes on the branch. Add your test beside these.
/// </summary>
public class EndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public EndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder => builder.UseEnvironment("Development"));
    }

    [Fact]
    public async Task Health_returns_200_OK_in_development()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("OK", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Ws_without_an_upgrade_request_is_refused_with_400()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/ws");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
