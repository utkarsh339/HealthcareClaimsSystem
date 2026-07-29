using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.Providers.Common;

public class ProviderResponse
{
    public Guid Id { get; set; }

    public string ProviderName { get; set; } = string.Empty;

    public ProviderType ProviderType { get; set; }

    public string NPI { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string ZipCode { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}