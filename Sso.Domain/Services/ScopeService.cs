using Sso.Domain.Entities;
using Sso.Domain.Enums;
using Sso.Domain.Repositories;

namespace Sso.Domain.Services
{
    public class ScopeService(IScopeRepository scopeRepository, IAuditLogRepository auditLogRepository)
    {
        public const string OpenIdScopeName = "openid";

        public const string ProfileScopeName = "profile";

        public const string EmailScopeName = "email";

        public Scope CreateScope(string name, string? description = null)
        {
            var scope = new Scope
            {
                Name = name,
                Description = description
            };

            scopeRepository.Add(scope);

            var audit = new AuditLog(relatedEntity: scope)
            {
                EventType = EventType.ScopeCreated
            };

            auditLogRepository.Add(audit);

            return scope;
        }
    }
}
