using Sso.Domain.Entities;
using Sso.Domain.Repositories;

namespace Sso.Infrastructure.Persistence.Repositories
{
    public class AuditLogRepository(SsoDbContext context) : IAuditLogRepository
    {
        public void Add(AuditLog auditLog)
        {
            context.AuditLogs.Add(auditLog);
        }
    }
}
