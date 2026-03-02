using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sso.Application.Authentication;
using Sso.Application.DTOs;

namespace Sso.Api.Controllers;

[ApiController]
[Route("api/[auth]")]
public calss AuthController(IAuthenticationService authenticationService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody]LoginRequest loginRequest, CancellationToken cancellationToken){
        var ipAdress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
        var result = await authenticationService.ValidateCredentialsAsync(loginRequest, ipAdress, userAgent, cancellationToken);

        if(result is null)
        {
            return BadRequest("Invalid credentials");
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
}