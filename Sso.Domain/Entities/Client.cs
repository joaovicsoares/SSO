namespace Sso.Domain.Entities
{
    public class Client
        : IEntityByGuid
    {
        public int Id { get; init; } = 0;

        public Guid Guid { get; init; } = Guid.NewGuid();

        public required string Name { get; init; }

        public required string ClientId { get; set; }

        public required string ClientSecret { get; set; }

        public required string RedirectUris { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        public IReadOnlyCollection<ClientPermission> ClientPermissions
            => clientPermissions;

        private readonly HashSet<ClientPermission> clientPermissions = [];

        public IReadOnlyCollection<Scope> Scopes => scopes;

        private readonly HashSet<Scope> scopes = [];
    }
}
