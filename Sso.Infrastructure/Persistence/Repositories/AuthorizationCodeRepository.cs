using Microsoft.EntityFrameworkCore;
using Sso.Domain.Entities;
using Sso.Domain.Repositories;

namespace Sso.Infrastructure.Persistence.Repositories;

public class AuthorizationCodeRepository : IAuthorizationCodeRepository
{
    private readonly SsoDbContext _context;

    public AuthorizationCodeRepository(SsoDbContext context)
    {
        _context = context;
    }

    public void Add(AuthorizationCode code)
    {
        _context.AuthorizationCodes.Add(code);
    }

    public async Task<AuthorizationCode?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        return await _context.AuthorizationCodes
            .Include(ac => ac.User)  // Eager loading para evitar N+1 queries
            .FirstOrDefaultAsync(ac => ac.Code == code, cancellationToken);
    }
}
