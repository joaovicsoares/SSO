using Sso.Domain.Entities;

namespace Sso.Domain.Repositories
{
    public interface IAuditLogRepository
    {
        void Add(AuditLog auditLog);
    }
}
