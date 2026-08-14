using HealthcareClaims.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.Claims.DeleteClaim;

public class DeleteClaimCommandHandler
    : IRequestHandler<DeleteClaimCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public DeleteClaimCommandHandler(
        IApplicationDbContext context,
        IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task Handle(
        DeleteClaimCommand request,
        CancellationToken cancellationToken)
    {
        var claim = await _context.Claims
            .FirstOrDefaultAsync(
                c => c.Id == request.Id,
                cancellationToken);

        if (claim is null)
        {
            throw new KeyNotFoundException("Claim not found.");
        }

        _context.Claims.Remove(claim);

        await _auditService.LogAsync(
            AuditAction.Delete,
            "Claim",
            claim.Id,
            $"Deleted claim {claim.Id}",
            cancellationToken: cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
