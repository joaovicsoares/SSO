namespace Sso.Domain.Entities
{
    public class UserPermission
    {
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

        public DateTime GrantedAt { get; init; } = DateTime.UtcNow;

        public int GrantedById { get; private init; }
        public required User GrantedBy
        {
            get;
            init
            {
                GrantedById = value.Id;

                field = value;
            }
        }
    }
}
