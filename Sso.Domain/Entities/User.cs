namespace Sso.Domain.Entities
{
    public class User
    {
        public int Id { get; init; } = 0;

        public Guid Guid { get; init; } = Guid.NewGuid();

        public required string Email { get; set; }

        public required string PasswordHash { get; set; }

        public string? Name { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        public IReadOnlyCollection<Role> Roles => roles;

        private readonly HashSet<Role> roles = [];

        public IReadOnlyCollection<UserPermission> UserPermissions
            => userPermissions;

        private readonly HashSet<UserPermission> userPermissions = [];
    }
}
