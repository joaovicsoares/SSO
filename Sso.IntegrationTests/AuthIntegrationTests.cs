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

        var setCookieHeaders = loginResponse.Headers.GetValues("Set-Cookie");
        var authCookie = setCookieHeaders.FirstOrDefault(c => c.StartsWith("SsoAuth="));
        var cookieHeaderValue = authCookie?.Split(';')[0];

        Assert.NotNull(cookieHeaderValue);

        // 2. Access protected endpoint (me)
        var meRequest1 = new HttpRequestMessage(HttpMethod.Get, "/api/auth/me");
        meRequest1.Headers.Add("Cookie", cookieHeaderValue);
        var meResponse1 = await _client.SendAsync(meRequest1);
        meResponse1.EnsureSuccessStatusCode();

        // 3. Logout
        var logoutRequest = new HttpRequestMessage(HttpMethod.Post, "/api/auth/logout");
        logoutRequest.Headers.Add("Cookie", cookieHeaderValue);
        var logoutResponse = await _client.SendAsync(logoutRequest);
        logoutResponse.EnsureSuccessStatusCode();

        if (logoutResponse.Headers.TryGetValues("Set-Cookie", out var logoutCookies))
        {
            var logoutAuthCookie = logoutCookies.FirstOrDefault(c => c.StartsWith("SsoAuth="));
            if (logoutAuthCookie != null)
            {
                cookieHeaderValue = logoutAuthCookie.Split(';')[0];
            }
            else 
            {
                cookieHeaderValue = null;
            }
        }
        else
        {
            cookieHeaderValue = null;
        }

        // 4. Access protected endpoint again (should fail)
        var meRequest2 = new HttpRequestMessage(HttpMethod.Get, "/api/auth/me");
        if (!string.IsNullOrEmpty(cookieHeaderValue))
        {
            meRequest2.Headers.Add("Cookie", cookieHeaderValue);
        }
        var meResponse2 = await _client.SendAsync(meRequest2);
        
        Assert.Equal(HttpStatusCode.Unauthorized, meResponse2.StatusCode);
    }
}
