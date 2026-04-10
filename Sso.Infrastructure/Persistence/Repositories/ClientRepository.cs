using Microsoft.EntityFrameworkCore;
using Sso.Domain.Entities;
using Sso.Domain.Repositories;

namespace Sso.Infrastructure.Persistence.Repositories;

public class ClientRepository(SsoDbContext context) : IClientRepository
{
    public async Task<Client?> GetByClientIdAsync(string clientId, CancellationToken cancellationToken = default)
    {
        return await context.Clients.FirstOrDefaultAsync(
            x => x.ClientId == clientId, cancellationToken
        );
    }

    public async Task AddAsync(Client client, CancellationToken cancellationToken = default)
    {
        await context.Clients.AddAsync(client, cancellationToken);
    }
}
