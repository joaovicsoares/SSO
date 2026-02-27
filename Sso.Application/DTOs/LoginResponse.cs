using Sso.Domain.ValueObjects;

namespace Sso.Application.DTOs;

public record LoginResponse(Guid Guid, Email Email, string? Name);