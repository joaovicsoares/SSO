namespace Sso.Domain.Entities
{
    public class User
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public required string Name { get; set; }

        public required string Email { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        public IReadOnlyCollection<Role> Roles => roles;
        private readonly HashSet<Role> roles = [];

        public IReadOnlyCollection<UserPermission> Permissions => permissions;
        private readonly HashSet<UserPermission> permissions = [];
    }
}
