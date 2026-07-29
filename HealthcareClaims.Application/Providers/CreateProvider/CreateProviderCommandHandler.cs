using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Providers.Common;
using HealthcareClaims.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Providers.CreateProvider;

public class CreateProviderCommandHandler
    : IRequestHandler<CreateProviderCommand, ProviderResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateProviderCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProviderResponse> Handle(
        CreateProviderCommand request,
        CancellationToken cancellationToken)
    {
        var providerExists = await _context.Providers
            .AnyAsync(
                p => p.NPI == request.NPI,
                cancellationToken);

        if (providerExists)
        {
            throw new InvalidOperationException(
                $"A provider with NPI '{request.NPI}' already exists.");
        }

        var provider = new Provider
        {
            Id = Guid.NewGuid(),
            ProviderName = request.ProviderName,
            ProviderType = request.ProviderType,
            NPI = request.NPI,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            City = request.City,
            State = request.State,
            ZipCode = request.ZipCode,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Providers.Add(provider);

        await _context.SaveChangesAsync(cancellationToken);

        return new ProviderResponse
        {
            Id = provider.Id,
            ProviderName = provider.ProviderName,
            ProviderType = provider.ProviderType,
            NPI = provider.NPI,
            Email = provider.Email,
            PhoneNumber = provider.PhoneNumber,
            Address = provider.Address,
            City = provider.City,
            State = provider.State,
            ZipCode = provider.ZipCode,
            IsActive = provider.IsActive,
            CreatedAt = provider.CreatedAt
        };
    }
}