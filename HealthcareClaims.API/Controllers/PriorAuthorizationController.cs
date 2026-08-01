using HealthcareClaims.API.Models.PriorAuthorizations;
using HealthcareClaims.Application.PriorAuthorizations.CreatePriorAuthorization;
using HealthcareClaims.Application.PriorAuthorizations.DeletePriorAuthorization;
using HealthcareClaims.Application.PriorAuthorizations.GetAllPriorAuthorizations;
using HealthcareClaims.Application.PriorAuthorizations.GetPriorAuthorizationById;
using HealthcareClaims.Application.PriorAuthorizations.UpdatePriorAuthorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareClaims.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PriorAuthorizationController : ControllerBase
{
    private readonly IMediator _mediator;

    public PriorAuthorizationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Policy = "PriorAuthorizationWriteAccess")]
    public async Task<IActionResult> Create(
        CreatePriorAuthorizationRequest request)
    {
        var command = new CreatePriorAuthorizationCommand
        {
            PatientId = request.PatientId,
            ProviderId = request.ProviderId,
            InsurancePolicyId = request.InsurancePolicyId,
            DiagnosisCode = request.DiagnosisCode,
            ProcedureCode = request.ProcedureCode,
            ServiceDescription = request.ServiceDescription
        };

        var response = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "PriorAuthorizationReadAccess")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _mediator.Send(
            new GetPriorAuthorizationByIdQuery
            {
                Id = id
            });

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "PriorAuthorizationReadAccess")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _mediator.Send(
            new GetAllPriorAuthorizationsQuery());

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "PriorAuthorizationWriteAccess")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdatePriorAuthorizationRequest request)
    {
        var command = new UpdatePriorAuthorizationCommand
        {
            Id = id,
            DiagnosisCode = request.DiagnosisCode,
            ProcedureCode = request.ProcedureCode,
            ServiceDescription = request.ServiceDescription,
            Status = request.Status,
            ReviewNotes = request.ReviewNotes
        };

        var response = await _mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "PriorAuthorizationWriteAccess")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(
            new DeletePriorAuthorizationCommand
            {
                Id = id
            });

        return NoContent();
    }
}