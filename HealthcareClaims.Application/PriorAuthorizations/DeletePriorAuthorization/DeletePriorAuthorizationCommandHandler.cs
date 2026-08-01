using HealthcareClaims.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.PriorAuthorizations.DeletePriorAuthorization;

public class DeletePriorAuthorizationCommandHandler
    : IRequestHandler<DeletePriorAuthorizationCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePriorAuthorizationCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
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

        await _context.SaveChangesAsync(cancellationToken);
    }
}