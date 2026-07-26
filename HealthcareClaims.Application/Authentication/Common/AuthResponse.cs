namespace HealthcareClaims.Application.Authentication.Common;

public class AuthResponse
{
    public Guid UserId { get; init; }

    public string FullName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string Role { get; init; } = string.Empty;

    public string Token { get; init; } = string.Empty;
}