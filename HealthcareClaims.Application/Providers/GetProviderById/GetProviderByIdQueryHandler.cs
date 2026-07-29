using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Providers.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Providers.GetProviderById;

public class GetProviderByIdQueryHandler
    : IRequestHandler<GetProviderByIdQuery, ProviderResponse?>
{
    private readonly IApplicationDbContext _context;

    public GetProviderByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProviderResponse?> Handle(
        GetProviderByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Providers
            .AsNoTracking()
            .Where(p => p.Id == request.Id)
            .Select(p => new ProviderResponse
            {
                Id = p.Id,
                ProviderName = p.ProviderName,
                ProviderType = p.ProviderType,
                NPI = p.NPI,
                Email = p.Email,
                PhoneNumber = p.PhoneNumber,
                Address = p.Address,
                City = p.City,
                State = p.State,
                ZipCode = p.ZipCode,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}