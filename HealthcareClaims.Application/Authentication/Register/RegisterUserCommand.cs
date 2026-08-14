using HealthcareClaims.Application.Authentication.Common;
using MediatR;

namespace HealthcareClaims.Application.Authentication.Register;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<AuthResponse>;
