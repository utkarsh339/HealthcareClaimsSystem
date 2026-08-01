using HealthcareClaims.Application.PriorAuthorizations.Common;
using MediatR;

namespace HealthcareClaims.Application.PriorAuthorizations.GetPriorAuthorizationById;

public class GetPriorAuthorizationByIdQuery : IRequest<PriorAuthorizationResponse>
{
    public Guid Id { get; set; }
}