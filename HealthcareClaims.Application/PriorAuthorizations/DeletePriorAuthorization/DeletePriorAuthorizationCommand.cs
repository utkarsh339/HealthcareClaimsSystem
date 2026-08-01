using MediatR;

namespace HealthcareClaims.Application.PriorAuthorizations.DeletePriorAuthorization;

public class DeletePriorAuthorizationCommand : IRequest
{
    public Guid Id { get; set; }
}