using HealthcareClaims.Application.Patients.Common;
using HealthcareClaims.Application.Patients.CreatePatient;
using HealthcareClaims.Application.Patients.GetPatientById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareClaims.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Policy = "PatientWriteAccess")]
    [ProducesResponseType(typeof(PatientResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreatePatient(
        CreatePatientRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreatePatientCommand(
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Gender,
            request.Email,
            request.PhoneNumber,
            request.Address);

        var response = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetPatientById),
            new { id = response.Id },
            response);
    }

    [HttpGet("{id:guid}", Name = nameof(GetPatientById))]
    [Authorize(Policy = "PatientReadAccess")]
    [ProducesResponseType(typeof(PatientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatientResponse>> GetPatientById(Guid id)
    {
        var patient = await _mediator.Send(new GetPatientByIdQuery(id));

        if (patient is null)
        {
            return NotFound();
        }

        return Ok(patient);
    }
}