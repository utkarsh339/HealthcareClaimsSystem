using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.API.Models.Providers;

public class CreateProviderRequest
{
    public string ProviderName { get; set; } = string.Empty;

    public ProviderType ProviderType { get; set; }

    public string NPI { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string ZipCode { get; set; } = string.Empty;
}