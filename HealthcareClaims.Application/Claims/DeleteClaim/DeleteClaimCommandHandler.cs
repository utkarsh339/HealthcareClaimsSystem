using HealthcareClaims.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Claims.DeleteClaim;

public class DeleteClaimCommandHandler
    : IRequestHandler<DeleteClaimCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteClaimCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
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

        await _context.SaveChangesAsync(cancellationToken);
    }
}