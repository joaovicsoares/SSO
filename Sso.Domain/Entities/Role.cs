namespace Sso.Domain.Entities
{
    public class Role
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public required string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        public IReadOnlyCollection<Permission> Permissions => permissions;
        private readonly HashSet<Permission> permissions = [];
    }
}
