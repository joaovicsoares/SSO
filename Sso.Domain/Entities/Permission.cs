namespace Sso.Domain.Entities
{
    public class Permission
        : IEntityByGuid
    {
        public int Id { get; init; } = 0;

        public Guid Guid { get; init; } = Guid.NewGuid();

        public required string Name { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }
}
