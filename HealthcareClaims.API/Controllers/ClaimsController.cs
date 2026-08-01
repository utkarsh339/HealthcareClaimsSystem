using HealthcareClaims.API.Models.Claims;
using HealthcareClaims.Application.Claims.CreateClaim;
using HealthcareClaims.Application.Claims.DeleteClaim;
using HealthcareClaims.Application.Claims.GetAllClaims;
using HealthcareClaims.Application.Claims.GetClaimById;
using HealthcareClaims.Application.Claims.UpdateClaim;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareClaims.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClaimsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClaimsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Policy = "ClaimWriteAccess")]
    public async Task<IActionResult> Create(
        CreateClaimRequest request)
    {
        var command = new CreateClaimCommand(
            request.PatientId,
            request.ProviderId,
            request.InsurancePolicyId,
            request.PriorAuthorizationId,
            request.DiagnosisCode,
            request.ProcedureCode,
            request.ServiceDate,
            request.BilledAmount,
            request.Remarks);

        var response = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "ClaimReadAccess")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _mediator.Send(
            new GetClaimByIdQuery(id));

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "ClaimReadAccess")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _mediator.Send(
            new GetAllClaimsQuery());

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "ClaimWriteAccess")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateClaimRequest request)
    {
        var command = new UpdateClaimCommand(
            id,
            request.DiagnosisCode,
            request.ProcedureCode,
            request.ServiceDate,
            request.BilledAmount,
            request.Remarks);

        var response = await _mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "ClaimWriteAccess")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(
            new DeleteClaimCommand(id));

        return NoContent();
    }
}