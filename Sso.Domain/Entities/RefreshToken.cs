namespace Sso.Domain.Entities
{
    public class RefreshToken
        : IEntityByGuid
    {
        public static TimeSpan DefaultExpirationTime => TimeSpan.FromMinutes(5);

        public int Id { get; init; } = 0;

        public Guid Guid { get; init; } = Guid.NewGuid();

        public required string Token { get; init; }

        public int UserId { get; private init; }
        public required User User
        {
            get;
            init
            {
                UserId = value.Id;
                field = value;
            }
        }

        public int ClientId { get; private init; }
        public required Client Client
        {
            get;
            init
            {
                ClientId = value.Id;
                field = value;
            }
        }

        public DateTime ExpiresAt { get; init; } = DateTime.UtcNow + DefaultExpirationTime;

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        public bool IsRevoked { get; set; } = false;
    }
}
