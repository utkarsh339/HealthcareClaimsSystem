using HealthcareClaims.Application.Providers.Common;
using HealthcareClaims.Domain.Enums;
using MediatR;

namespace HealthcareClaims.Application.Providers.UpdateProvider;

public sealed record UpdateProviderCommand(
    Guid Id,
    string ProviderName,
    ProviderType ProviderType,
    string NPI,
    string Email,
    string PhoneNumber,
    string Address,
    string City,
    string State,
    string ZipCode,
    bool IsActive
) : IRequest<ProviderResponse>;