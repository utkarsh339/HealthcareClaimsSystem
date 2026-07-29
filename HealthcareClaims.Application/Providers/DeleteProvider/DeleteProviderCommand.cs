using MediatR;

namespace HealthcareClaims.Application.Providers.DeleteProvider;

public sealed record DeleteProviderCommand(Guid Id) : IRequest;