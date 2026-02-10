namespace Sso.Domain.Entities
{
    public class UserPermission
    {
        public Guid UserId { get; private init; }

        public required User User
        {
            get;
            init
            {
                UserId = value.Id;

                field = value;
            }
        }

        public Guid PermissionId { get; private init; }

        public required Permission Permission
        {
            get;
            init
            {
                PermissionId = value.Id;

                field = value;
            }
        }

        public DateTime GrantedAt { get; init; } = DateTime.UtcNow;

        public required User GrantedBy { get; init; }
    }
}
