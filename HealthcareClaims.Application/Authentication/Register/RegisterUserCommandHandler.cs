using HealthcareClaims.Application.Authentication.Common;
using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Exceptions;
using HealthcareClaims.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.Authentication.Register;

public class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IAuditService _auditService;

    public RegisterUserCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IAuditService auditService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _auditService = auditService;
    }

    public async Task<AuthResponse> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var emailExists = await _context.Users
            .AnyAsync(
                u => u.Email == request.Email,
                cancellationToken);

        if (emailExists)
        {
            throw new DuplicateEmailException(request.Email);
        }

        var hashedPassword =
            _passwordHasher.HashPassword(request.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = hashedPassword,
            // Public self-registration is always ProviderStaff. Privileged roles
            // are provisioned through an authenticated administrative workflow.
            Role = UserRole.ProviderStaff,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);

        await _auditService.LogAsync(
            AuditAction.Create,
            "User",
            user.Id,
            $"Registered user: {user.Email}",
            user.Id,
            user.Email,
            cancellationToken: cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user);

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
