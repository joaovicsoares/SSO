using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthService = Sso.Application.Authentication.IAuthenticationService;
using Microsoft.AspNetCore.RateLimiting;
using Sso.Application.DTOs;

namespace Sso.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authenticationService) : ControllerBase
{
    [HttpPost("login")]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> LoginAsync([FromBody]LoginRequest loginRequest, CancellationToken cancellationToken){
        var ipAdress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
        var result = await authenticationService.ValidateCredentialsAsync(loginRequest, ipAdress, userAgent, cancellationToken);

        if(result is null)
        {
            return Unauthorized("Credenciais inválidas");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.Guid),
            new(ClaimTypes.Email, result.Email),
        };

        if (result.Name is not null)
        {
            claims.Add(new Claim(ClaimTypes.Name, result.Name));
        }

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);
            
        return Ok(result);
    }

    [Authorize]
    [HttpPost("Logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        // Sign out from cookie authentication
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        // For JWT clients, they should discard the token on their side
        // Optionally, you could implement token revocation here

        return Ok(new { message = "Logout realizado com sucesso" });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var guid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = User.FindFirstValue(ClaimTypes.Email);
        var name = User.FindFirstValue(ClaimTypes.Name);

        return Ok(new { guid, email, name });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokenAsync(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

        var result = await authenticationService.RefreshTokenAsync(
            request.RefreshToken, ipAddress, userAgent, cancellationToken);

        if (result is null)
        {
            return Unauthorized(new { message = "Refresh token inválido ou expirado" });
        }

        // Optionally set cookie for web clients
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.Guid),
            new(ClaimTypes.Email, result.Email),
        };

        if (result.Name is not null)
        {
            claims.Add(new Claim(ClaimTypes.Name, result.Name));
        }

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        return Ok(result);
    }

    [HttpPost("register")]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> RegisterAsync(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = HttpContext.Request.Headers.UserAgent.ToString();
        
        var result = await authenticationService.RegisterUserAsync(
            request, ipAddress, userAgent, cancellationToken);

        if (result is null)
        {
            return BadRequest(new { message = "Email já cadastrado ou inválido" });
        }

        // Set cookie for web clients
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.Guid),
            new(ClaimTypes.Email, result.Email),
        };

        if (result.Name is not null)
        {
            claims.Add(new Claim(ClaimTypes.Name, result.Name));
        }

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        return Ok(result);
    }
}