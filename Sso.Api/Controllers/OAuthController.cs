using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sso.Application.Authentication;

namespace Sso.Api.Controllers;

[ApiController]
public class OAuthController : ControllerBase
{
    private readonly IOAuthService _oauthService;

    public OAuthController(IOAuthService oauthService)
    {
        _oauthService = oauthService;
    }

    [Authorize]
    [HttpGet("authorize")]
    public async Task<IActionResult> Authorize(
        [FromQuery] string? code_challenge,
        [FromQuery] string? code_challenge_method,
        [FromQuery] string? state,
        CancellationToken cancellationToken)
    {
        var userGuid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (userGuid == null || !Guid.TryParse(userGuid, out var userId))
        {
            return Unauthorized(new 
            { 
                error = "invalid_request", 
                error_description = "User not authenticated" 
            });
        }

        var code = await _oauthService.GenerateAuthorizationCodeAsync(
            userId,
            code_challenge,
            code_challenge_method,
            cancellationToken);

        return Ok(new { code, state });
    }

    [HttpPost("token")]
    public async Task<IActionResult> Token(
        [FromForm] string grant_type,
        [FromForm] string? code,
        [FromForm] string? code_verifier,
        CancellationToken cancellationToken)
    {
        if (grant_type != "authorization_code")
        {
            return BadRequest(new 
            { 
                error = "unsupported_grant_type",
                error_description = "Only authorization_code grant type is supported"
            });
        }

        if (string.IsNullOrEmpty(code))
        {
            return BadRequest(new 
            { 
                error = "invalid_request", 
                error_description = "code parameter is required" 
            });
        }

        var tokens = await _oauthService.ExchangeCodeForTokensAsync(
            code,
            code_verifier,
            cancellationToken);

        if (tokens == null)
        {
            return BadRequest(new 
            { 
                error = "invalid_grant", 
                error_description = "Invalid, expired, or already used authorization code" 
            });
        }

        return Ok(new
        {
            access_token = tokens.AccessToken,
            id_token = tokens.IdToken,
            refresh_token = tokens.RefreshToken,
            token_type = tokens.TokenType,
            expires_in = tokens.ExpiresIn
        });
    }
}
