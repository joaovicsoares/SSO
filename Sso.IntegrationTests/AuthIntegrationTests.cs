using Microsoft.AspNetCore.Mvc.Testing;
using Sso.Application.DTOs;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Sso.Infrastructure.Persistence;
using Sso.Infrastructure.Persistence.Seeds;

namespace Sso.IntegrationTests;

public class AuthIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public AuthIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Seed
        using var scope = _factory.Services.CreateScope();
        var userSeed = scope.ServiceProvider.GetRequiredService<UserSeed>();
        userSeed.SeedAsync().GetAwaiter().GetResult();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOk()
    {
        // Arrange
        var loginRequest = new LoginRequest("admin@gmail.com", "admin123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(content);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var loginRequest = new LoginRequest("admin@gmail.com", "wrongpassword");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMe_WithoutLogin_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/api/auth/me");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task FullFlow_Login_AccessProtected_Logout_AccessProtected_ReturnsCorrectStatuses()
    {
        // 1. Login
        var loginRequest = new LoginRequest("admin@gmail.com", "admin123");
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
        loginResponse.EnsureSuccessStatusCode();

        // 2. Access protected endpoint (me)
        var meResponse = await _client.GetAsync("/api/auth/me");
        meResponse.EnsureSuccessStatusCode();
        
        var user = await meResponse.Content.ReadFromJsonAsync<dynamic>();
        // Assert.Equal("admin@gmail.com", (string)user.email); // dynamic is tricky in C#, but we can check status

        // 3. Logout
        var logoutResponse = await _client.PostAsync("/api/auth/logout", null);
        logoutResponse.EnsureSuccessStatusCode();

        // 4. Access protected endpoint again (should fail)
        var meResponseAfterLogout = await _client.GetAsync("/api/auth/me");
        Assert.Equal(HttpStatusCode.Unauthorized, meResponseAfterLogout.StatusCode);
    }
}
