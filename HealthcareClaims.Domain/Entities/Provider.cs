namespace HealthcareClaims.Domain.Entities;

public class Provider
{
    public Guid Id { get; set; }

    public string ProviderName { get; set; } = string.Empty;

    public string ProviderType { get; set; } = string.Empty;

    public string LicenseNumber { get; set; } = string.Empty;

    public string ContactPerson { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}