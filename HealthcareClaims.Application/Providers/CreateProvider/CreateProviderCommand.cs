using HealthcareClaims.Application.Providers.Common;
using HealthcareClaims.Domain.Enums;
using MediatR;

namespace HealthcareClaims.Application.Providers.CreateProvider;

public sealed record CreateProviderCommand(
    string ProviderName,
    ProviderType ProviderType,
    string NPI,
    string Email,
    string PhoneNumber,
    string Address,
    string City,
    string State,
    string ZipCode
) : IRequest<ProviderResponse>;