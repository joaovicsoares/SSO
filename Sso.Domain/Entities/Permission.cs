namespace Sso.Domain.Entities
{
    public class Permission
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public required string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }
}
