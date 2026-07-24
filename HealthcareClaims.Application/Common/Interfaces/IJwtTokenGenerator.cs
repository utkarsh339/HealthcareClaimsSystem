using HealthcareClaims.Domain.Entities;

namespace HealthcareClaims.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}