using HealthcareClaims.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.InsurancePolicies.DeleteInsurancePolicy;

public class DeleteInsurancePolicyCommandHandler
    : IRequestHandler<DeleteInsurancePolicyCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteInsurancePolicyCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
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

        await _context.SaveChangesAsync(cancellationToken);
    }
}