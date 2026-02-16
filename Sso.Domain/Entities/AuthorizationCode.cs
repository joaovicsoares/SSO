namespace Sso.Domain.Entities
{
    public class AuthorizationCode
        : IEntityByGuid
    {
        public static TimeSpan DefaultExpirationTime => TimeSpan.FromHours(1);

        public int Id { get; init; } = 0;

        public Guid Guid { get; init; } = Guid.NewGuid();

        public required string Code { get; init; }

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

        public required string RedirectUri { get; set; }

        public DateTime ExpiresAt { get; init; } = DateTime.UtcNow + DefaultExpirationTime;

        public bool IsUsed { get; set; } = false;
    }
}
