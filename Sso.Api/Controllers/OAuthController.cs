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

    /// <summary>
    /// OAuth 2.0 Authorization Endpoint
    /// Requer que o usuário esteja autenticado (cookie)
    /// </summary>
    /// <param name="code_challenge">PKCE code challenge (SHA256 do code_verifier)</param>
    /// <param name="code_challenge_method">Método PKCE: S256 ou plain</param>
    /// <param name="state">Estado para CSRF protection (opcional)</param>
    [Authorize] // Requer cookie de autenticação
    [HttpGet("authorize")]
    public async Task<IActionResult> Authorize(
        [FromQuery] string? code_challenge,
        [FromQuery] string? code_challenge_method,
        [FromQuery] string? state,
        CancellationToken cancellationToken)
    {
        // Obter GUID do usuário autenticado do cookie
        var userGuid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (userGuid == null || !Guid.TryParse(userGuid, out var userId))
        {
            return Unauthorized(new 
            { 
                error = "invalid_request", 
                error_description = "User not authenticated" 
            });
        }

        // Gerar authorization code vinculado ao usuário
        var code = await _oauthService.GenerateAuthorizationCodeAsync(
            userId,
            code_challenge,
            code_challenge_method,
            cancellationToken);

        // Retornar código
        // Em produção real, você redirecionaria para redirect_uri do cliente
        // Exemplo: return Redirect($"{redirect_uri}?code={code}&state={state}");
        return Ok(new { code, state });
    }

    /// <summary>
    /// OAuth 2.0 Token Endpoint
    /// Troca authorization code por tokens
    /// </summary>
    /// <param name="grant_type">Tipo de grant (deve ser "authorization_code")</param>
    /// <param name="code">Authorization code recebido do /authorize</param>
    /// <param name="code_verifier">PKCE code verifier (string original)</param>
    [HttpPost("token")]
    public async Task<IActionResult> Token(
        [FromForm] string grant_type,
        [FromForm] string? code,
        [FromForm] string? code_verifier,
        CancellationToken cancellationToken)
    {
        // 1. Validar grant_type
        if (grant_type != "authorization_code")
        {
            return BadRequest(new 
            { 
                error = "unsupported_grant_type",
                error_description = "Only authorization_code grant type is supported"
            });
        }

        // 2. Validar que code foi fornecido
        if (string.IsNullOrEmpty(code))
        {
            return BadRequest(new 
            { 
                error = "invalid_request", 
                error_description = "code parameter is required" 
            });
        }

        // 3. Trocar código por tokens
        var tokens = await _oauthService.ExchangeCodeForTokensAsync(
            code,
            code_verifier,
            cancellationToken);

        // 4. Validar se a troca foi bem-sucedida
        if (tokens == null)
        {
            return BadRequest(new 
            { 
                error = "invalid_grant", 
                error_description = "Invalid, expired, or already used authorization code" 
            });
        }

        // 5. Retornar tokens no formato OAuth 2.0 padrão
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
