using HealthcareClaims.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Providers.DeleteProvider;

public class DeleteProviderCommandHandler
    : IRequestHandler<DeleteProviderCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProviderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
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

        await _context.SaveChangesAsync(cancellationToken);
    }
}