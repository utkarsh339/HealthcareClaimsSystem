
using HealthcareClaims.Domain.Enums;
namespace HealthcareClaims.Application.Authentication.Register;


public record RegisterUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    UserRole Role);