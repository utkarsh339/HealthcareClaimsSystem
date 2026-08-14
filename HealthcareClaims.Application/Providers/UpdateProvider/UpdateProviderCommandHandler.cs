using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Providers.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.Providers.UpdateProvider;

public class UpdateProviderCommandHandler
    : IRequestHandler<UpdateProviderCommand, ProviderResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public UpdateProviderCommandHandler(IApplicationDbContext context,
        IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task<ProviderResponse> Handle(
        UpdateProviderCommand request,
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

        var duplicateNpi = await _context.Providers
            .AnyAsync(
                p => p.Id != request.Id &&
                     p.NPI == request.NPI,
                cancellationToken);

        if (duplicateNpi)
        {
            throw new InvalidOperationException(
                $"A provider with NPI '{request.NPI}' already exists.");
        }

        provider.ProviderName = request.ProviderName;
        provider.ProviderType = request.ProviderType;
        provider.NPI = request.NPI;
        provider.Email = request.Email;
        provider.PhoneNumber = request.PhoneNumber;
        provider.Address = request.Address;
        provider.City = request.City;
        provider.State = request.State;
        provider.ZipCode = request.ZipCode;
        provider.IsActive = request.IsActive;

        await _auditService.LogAsync(
            AuditAction.Update,
            "Provider",
            provider.Id,
            $"Updated provider {provider.Id}",
            cancellationToken: cancellationToken);

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
