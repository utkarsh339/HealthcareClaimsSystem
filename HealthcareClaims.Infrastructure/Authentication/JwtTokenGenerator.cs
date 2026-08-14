using System.IdentityModel.Tokens.Jwt;
using JwtClaim = System.Security.Claims.Claim;
using System.Security.Claims;
using System.Text;
using HealthcareClaims.Application.Common.Authentication;
using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HealthcareClaims.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _settings;

    public JwtTokenGenerator(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
{
    new JwtClaim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new JwtClaim(ClaimTypes.Email, user.Email),
    new JwtClaim(ClaimTypes.Role, user.Role.ToString()),

    // Optional but useful
    new JwtClaim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
    new JwtClaim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_settings.SecretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}