using Sso.Domain.Enums;

namespace Sso.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; init; } = 0;

        public required EventType EventType { get; init; }

        public EntityType? EntityType { get; init; }

        public string? EntityId { get; init; }

        public int? UserId { get; private init; }
        public User? User
        {
            get;
            init
            {
                UserId = value?.Id;
                field = value;
            }
        }

        public int? ClientId { get; private init; }
        public Client? Client
        {
            get;
            init
            {
                ClientId = value?.Id;
                field = value;
            }
        }

        public DateTime Timestamp { get; init; } = DateTime.UtcNow;

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }

        public object? Data { get; init; }
    }
}
