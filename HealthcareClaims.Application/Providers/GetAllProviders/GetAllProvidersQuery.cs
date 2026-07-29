using HealthcareClaims.Application.Providers.Common;
using MediatR;

namespace HealthcareClaims.Application.Providers.GetAllProviders;

public sealed record GetAllProvidersQuery
    : IRequest<IReadOnlyList<ProviderResponse>>;