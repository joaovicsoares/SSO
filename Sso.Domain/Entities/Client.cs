namespace Sso.Domain.Entities
{
    public class Client
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public required string Name { get; init; }
    }
}
