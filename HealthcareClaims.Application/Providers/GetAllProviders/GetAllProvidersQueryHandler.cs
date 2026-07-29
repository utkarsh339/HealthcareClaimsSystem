using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Providers.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Providers.GetAllProviders;

public class GetAllProvidersQueryHandler
    : IRequestHandler<GetAllProvidersQuery, IReadOnlyList<ProviderResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetAllProvidersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ProviderResponse>> Handle(
        GetAllProvidersQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Providers
            .AsNoTracking()
            .OrderBy(p => p.ProviderName)
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
            .ToListAsync(cancellationToken);
    }
}