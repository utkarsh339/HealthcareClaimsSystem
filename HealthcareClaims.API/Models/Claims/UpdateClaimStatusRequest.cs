using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.API.Models.Claims;

public class UpdateClaimStatusRequest
{
    public ClaimStatus Status { get; set; }
    public decimal ApprovedAmount { get; set; }
}
