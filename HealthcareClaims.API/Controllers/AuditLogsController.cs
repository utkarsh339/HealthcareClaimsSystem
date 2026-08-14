using HealthcareClaims.Application.AuditLogs.GetAllAuditLogs;
using HealthcareClaims.Application.AuditLogs.GetAuditLogById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareClaims.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditLogsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuditLogsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "AuditLogReadAccess")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _mediator.Send(
            new GetAuditLogByIdQuery(id));

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AuditLogReadAccess")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _mediator.Send(
            new GetAllAuditLogsQuery());

        return Ok(response);
    }


}