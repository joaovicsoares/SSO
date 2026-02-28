using Sso.Domain.Repositories;
using Sso.Domain.Services;
using Sso.Application.Persistence;
using Sso.Application.DTOs;
using Sso.Domain.ValueObjects;

namespace Sso.Application.Authentication;

public class AuthenticationService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    // IAuditLogRepository auditLogRepository,
    // IUnitOfWork unitOfWork
    ) : IAuthenticationService
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
        if (!Email.IsValid(loginRequest.Email))
        {
            //adicionar Log no futuro
            passwordHasher.VerifyPassword(loginRequest.Password, DummyHash);
            return null;
        }
        var email = new Email(loginRequest.Email);
        var user = await userRepository.GetByEmailAsync(email, cancellationToken);

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

        return new LoginResponse(user.Guid.ToString(), user.Email.Value, user.Name);
    }
}
