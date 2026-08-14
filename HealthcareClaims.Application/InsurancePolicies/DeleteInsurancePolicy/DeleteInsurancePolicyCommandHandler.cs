using HealthcareClaims.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.InsurancePolicies.DeleteInsurancePolicy;

public class DeleteInsurancePolicyCommandHandler
    : IRequestHandler<DeleteInsurancePolicyCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public DeleteInsurancePolicyCommandHandler(
        IApplicationDbContext context,
        IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task Handle(
        DeleteInsurancePolicyCommand request,
        CancellationToken cancellationToken)
    {
        var insurancePolicy = await _context.InsurancePolicies
            .FirstOrDefaultAsync(
                ip => ip.Id == request.Id,
                cancellationToken);

        if (insurancePolicy is null)
        {
            throw new KeyNotFoundException("Insurance policy not found.");
        }

        _context.InsurancePolicies.Remove(insurancePolicy);

        await _auditService.LogAsync(
            AuditAction.Delete,
            "InsurancePolicy",
            insurancePolicy.Id,
            $"Deleted insurance policy {insurancePolicy.Id}",
            cancellationToken: cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
