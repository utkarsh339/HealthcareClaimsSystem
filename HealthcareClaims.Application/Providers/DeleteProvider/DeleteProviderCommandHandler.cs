using HealthcareClaims.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.Providers.DeleteProvider;

public class DeleteProviderCommandHandler
    : IRequestHandler<DeleteProviderCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public DeleteProviderCommandHandler(IApplicationDbContext context,
        IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task Handle(
        DeleteProviderCommand request,
        CancellationToken cancellationToken)
    {
        var provider = await _context.Providers
            .FirstOrDefaultAsync(
                p => p.Id == request.Id,
                cancellationToken);

        if (provider is null)
        {
            throw new KeyNotFoundException("Provider not found.");
        }

        _context.Providers.Remove(provider);

        await _auditService.LogAsync(
            AuditAction.Delete,
            "Provider",
            provider.Id,
            $"Deleted provider {provider.Id}",
            cancellationToken: cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
