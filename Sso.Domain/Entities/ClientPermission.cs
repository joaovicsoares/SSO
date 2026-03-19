namespace Sso.Domain.Entities
{
    public class ClientPermission
    {
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

        public int PermissionId { get; private init; }
        public required Permission Permission
        {
            get;
            init
            {
                PermissionId = value.Id;
                field = value;
            }
        }

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }
}
