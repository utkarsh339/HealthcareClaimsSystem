using HealthcareClaims.Application.Providers.Common;
using MediatR;

namespace HealthcareClaims.Application.Providers.GetProviderById;

public sealed record GetProviderByIdQuery(Guid Id)
    : IRequest<ProviderResponse?>;