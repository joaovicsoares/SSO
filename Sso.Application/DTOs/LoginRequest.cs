using Sso.Domain.ValueObjects;

namespace Sso.Application.DTOs;

public record LoginRequest(Email Email, string Password);