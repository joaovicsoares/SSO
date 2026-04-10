using Sso.Domain.Entities;

namespace Sso.Domain.Repositories;

public interface IClientRepository 
{
    Task<Client?> GetByClientIdAsync(string clientId, CancellationToken cancellationToken = default);
    Task AddAsync(Client client, CancellationToken cancellationToken = default);
}
