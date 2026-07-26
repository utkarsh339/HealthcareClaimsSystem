using HealthcareClaims.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HealthcareClaims.Infrastructure.Authentication;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _passwordHasher = new();

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(null!, password);
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(
            null!,
            hashedPassword,
            password);

        return result == PasswordVerificationResult.Success;
    }
}