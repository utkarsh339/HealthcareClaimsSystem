using HealthcareClaims.Application.Authentication.Common;
using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Authentication.Login;

public class LoginHandler
    : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(
                u => u.Email == request.Email,
                cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        var isPasswordValid =
            _passwordHasher.VerifyPassword(
                user.PasswordHash,
                request.Password);

        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        var token =
            _jwtTokenGenerator.GenerateToken(user);

        return new AuthResponse
        {
            UserId = user.Id,
            FullName = $"{user.FirstName} {user.LastName}",
            Email = user.Email,
            Role = user.Role.ToString(),
            Token = token
        };
    }
}