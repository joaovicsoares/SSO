using System.Security.Claims;
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
    public async Task<IActionResult> LoginAsync([FromBody]LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        var ipAdress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
        var result = await authenticationService.ValidateCredentialsAsync(loginRequest, ipAdress, userAgent, cancellationToken);

        if(result is null)
        {
            return Unauthorized(new { error = "invalid_credentials", message = "Credenciais inválidas" });
        }

        return Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetMe()
    {
        var guid = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = User.FindFirstValue("email") ?? User.FindFirstValue(ClaimTypes.Email);
        var name = User.FindFirstValue("name") ?? User.FindFirstValue(ClaimTypes.Name);

        return Ok(new { guid, email, name });
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
            return BadRequest(new { error = "registration_failed", message = "Email já cadastrado ou inválido" });
        }

        return Ok(result);
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
            return Unauthorized(new { error = "invalid_token", message = "Refresh token inválido ou expirado" });
        }

        return Ok(result);
    }
}
