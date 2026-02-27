using Sso.Domain.Repositories;
using Sso.Domain.Services;
using Sso.Application.Persistence;
using Sso.Application.DTOs;

namespace Sso.Application.Authentication;

public class AuthenticationService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IAuditLogRepository auditLogRepository,
    IUnitOfWork unitOfWork)
{
    // Hash pré-computado usado para manter tempo de resposta constante
    // quando o email não existe (prevenção de timing attack)
    private const string DummyHash =
        "$argon2id$v=19$m=65536,t=4,p=1$AAAAAAAAAAAAAAAAAAAAAA==$AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";

    public async Task<LoginResponse?> ValidateCredentialsAsync(
        LoginRequest loginRequest,
        string? ipAddress = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(loginRequest.Email, cancellationToken);

        if (user is null)
        {
            passwordHasher.VerifyPassword(loginRequest.Password, DummyHash);
            //adicionar log no futuro
            return null;
        }

        if (!user.IsActive)
        {
            //adicionar log no futuro
            return null;
        }

        if (!passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash))
        {
            //adicionar log no futuro
            return null;
        }

        return new LoginResponse(user.Guid, user.Email, user.Name);
    }
}
