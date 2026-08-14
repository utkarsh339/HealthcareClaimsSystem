using HealthcareClaims.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.PriorAuthorizations.DeletePriorAuthorization;

public class DeletePriorAuthorizationCommandHandler
    : IRequestHandler<DeletePriorAuthorizationCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public DeletePriorAuthorizationCommandHandler(
        IApplicationDbContext context,
        IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task Handle(
        DeletePriorAuthorizationCommand request,
        CancellationToken cancellationToken)
    {
        var priorAuthorization = await _context.PriorAuthorizations
            .FirstOrDefaultAsync(
                pa => pa.Id == request.Id,
                cancellationToken);

        if (priorAuthorization is null)
        {
            throw new KeyNotFoundException("Prior authorization not found.");
        }

        _context.PriorAuthorizations.Remove(priorAuthorization);

        await _auditService.LogAsync(
            AuditAction.Delete,
            "PriorAuthorization",
            priorAuthorization.Id,
            $"Deleted prior authorization {priorAuthorization.Id}",
            cancellationToken: cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
