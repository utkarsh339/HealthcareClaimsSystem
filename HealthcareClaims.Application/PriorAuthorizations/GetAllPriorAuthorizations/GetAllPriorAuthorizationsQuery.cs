using HealthcareClaims.Application.PriorAuthorizations.Common;
using MediatR;

namespace HealthcareClaims.Application.PriorAuthorizations.GetAllPriorAuthorizations;

public class GetAllPriorAuthorizationsQuery
    : IRequest<IReadOnlyList<PriorAuthorizationResponse>>
{
}