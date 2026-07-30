using HealthcareClaims.API.Models.InsurancePolicies;
using HealthcareClaims.Application.InsurancePolicies.Common;
using HealthcareClaims.Application.InsurancePolicies.CreateInsurancePolicy;
using HealthcareClaims.Application.InsurancePolicies.DeleteInsurancePolicy;
using HealthcareClaims.Application.InsurancePolicies.GetAllInsurancePolicies;
using HealthcareClaims.Application.InsurancePolicies.GetInsurancePolicyById;
using HealthcareClaims.Application.InsurancePolicies.UpdateInsurancePolicy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareClaims.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InsurancePolicyController : ControllerBase
{
    private readonly IMediator _mediator;

    public InsurancePolicyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Policy = "InsurancePolicyWriteAccess")]
    [ProducesResponseType(typeof(InsurancePolicyResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateInsurancePolicy(
        CreateInsurancePolicyRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateInsurancePolicyCommand(
            request.PatientId,
            request.InsuranceCompany,
            request.PolicyNumber,
            request.MemberId,
            request.GroupNumber,
            request.PlanName,
            request.EffectiveDate,
            request.ExpirationDate,
            request.IsActive);

        var response = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetInsurancePolicyById),
            new { id = response.Id },
            response);
    }

    [HttpGet("{id:guid}", Name = nameof(GetInsurancePolicyById))]
    [Authorize(Policy = "InsurancePolicyReadAccess")]
    [ProducesResponseType(typeof(InsurancePolicyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InsurancePolicyResponse>> GetInsurancePolicyById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetInsurancePolicyByIdQuery(id),
            cancellationToken);

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "InsurancePolicyReadAccess")]
    [ProducesResponseType(typeof(IReadOnlyList<InsurancePolicyResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<InsurancePolicyResponse>>> GetAllInsurancePolicies(
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetAllInsurancePoliciesQuery(),
            cancellationToken);

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "InsurancePolicyWriteAccess")]
    [ProducesResponseType(typeof(InsurancePolicyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InsurancePolicyResponse>> UpdateInsurancePolicy(
        Guid id,
        UpdateInsurancePolicyRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateInsurancePolicyCommand(
            id,
            request.InsuranceCompany,
            request.PolicyNumber,
            request.MemberId,
            request.GroupNumber,
            request.PlanName,
            request.EffectiveDate,
            request.ExpirationDate,
            request.IsActive);

        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "InsurancePolicyWriteAccess")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteInsurancePolicy(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteInsurancePolicyCommand(id),
            cancellationToken);

        return NoContent();
    }
}