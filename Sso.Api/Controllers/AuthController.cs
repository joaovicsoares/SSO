using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthService = Sso.Application.Authentication.IAuthenticationService;
using Sso.Application.DTOs;

namespace Sso.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authenticationService) : ControllerBase
{
    [HttpPost("login")]
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
            
        return Ok(new { message = "Login realizado com sucesso" });
    }

    [Authorize]
    [HttpPost("Logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

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
}