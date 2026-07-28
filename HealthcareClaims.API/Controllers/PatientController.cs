using HealthcareClaims.Application.Patients.Common;
using HealthcareClaims.Application.Patients.CreatePatient;
using HealthcareClaims.Application.Patients.DeletePatient;
using HealthcareClaims.Application.Patients.GetAllPatients;
using HealthcareClaims.Application.Patients.GetPatientById;
using HealthcareClaims.Application.Patients.UpdatePatient;
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

    [HttpGet]
    [Authorize(Policy = "PatientReadAccess")]
    [ProducesResponseType(typeof(IReadOnlyList<PatientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<PatientResponse>>> GetAllPatients(
    CancellationToken cancellationToken)
    {
        var patients = await _mediator.Send(
            new GetAllPatientsQuery(),
            cancellationToken);

        return Ok(patients);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "PatientWriteAccess")]
    [ProducesResponseType(typeof(PatientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatientResponse>> UpdatePatient(
    Guid id,
    UpdatePatientRequest request,
    CancellationToken cancellationToken)
    {
        var command = new UpdatePatientCommand(
            id,
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Gender,
            request.Email,
            request.PhoneNumber,
            request.Address);

        var patient = await _mediator.Send(command, cancellationToken);

        if (patient is null)
        {
            return NotFound();
        }

        return Ok(patient);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "PatientWriteAccess")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePatient(
    Guid id,
    CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeletePatientCommand(id),
            cancellationToken);

        return NoContent();
    }
}