using Microsoft.EntityFrameworkCore;
using Sso.Domain.Entities;
using Sso.Domain.Repositories;

namespace Sso.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly SsoDbContext _context;

    public RefreshTokenRepository(SsoDbContext context)
    {
        _context = context;
    }

    public void Add(RefreshToken token)
    {
        _context.RefreshTokens.Add(token);
    }

    public async Task<RefreshToken?> GetByTokenAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.User)  // Carrega o User junto para evitar queries extras
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }
}
