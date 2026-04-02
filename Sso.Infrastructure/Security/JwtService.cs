using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sso.Application.Authentication;
using Sso.Domain.Entities;

namespace Sso.Infrastructure.Security;

public class JwtService : IJwtService
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _accessTokenExpiration;
    private readonly RSA _rsa;

    public JwtService(IConfiguration configuration)
    {
        _issuer = configuration["Jwt:Issuer"] 
            ?? throw new InvalidOperationException("Jwt:Issuer not configured");
        _audience = configuration["Jwt:Audience"] 
            ?? throw new InvalidOperationException("Jwt:Audience not configured");
        _accessTokenExpiration = configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes", 15);
        
        var privateKey = configuration["Jwt:PrivateKey"] 
            ?? throw new InvalidOperationException("Jwt:PrivateKey not configured");
        
        _rsa = RSA.Create();
        _rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
    }

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Guid.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.Value),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        if (user.Name != null)
            claims.Add(new(JwtRegisteredClaimNames.Name, user.Name));

        var credentials = new SigningCredentials(
            new RsaSecurityKey(_rsa),
            SecurityAlgorithms.RsaSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_accessTokenExpiration),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateIdToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Guid.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.Value),
            new("email_verified", "true"),
        };

        if (user.Name != null)
            claims.Add(new(JwtRegisteredClaimNames.Name, user.Name));

        var credentials = new SigningCredentials(
            new RsaSecurityKey(_rsa),
            SecurityAlgorithms.RsaSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_accessTokenExpiration),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
