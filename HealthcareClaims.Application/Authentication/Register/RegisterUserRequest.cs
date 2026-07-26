
namespace HealthcareClaims.Application.Authentication.Register;
using HealthcareClaims.Domain.Enums;

public record RegisterUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    UserRole Role);