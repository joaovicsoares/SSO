using Sso.Domain.Enums;

namespace Sso.Domain.Entities
{
    public class AuditLog
    {
        public AuditLog()
        {

        }

        public AuditLog(IEntityByGuid relatedEntity)
        {
            EntityGuid = relatedEntity.Guid;
        }

        public int Id { get; init; } = 0;

        public required EventType EventType { get; init; }

        public Guid? EntityGuid { get; private init; }

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
