using HealthcareClaims.Application.Authentication.Common;
using MediatR;

namespace HealthcareClaims.Application.Authentication.Login
{
    public record LoginCommand(
    string Email,
    string Password)
    : IRequest<AuthResponse>;
}
