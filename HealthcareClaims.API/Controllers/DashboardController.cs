using HealthcareClaims.Application.Dashboard.GetClaimsStatistics;
using HealthcareClaims.Application.Dashboard.GetDashboardSummary;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareClaims.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "DashboardReadAccess")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetDashboardSummaryQuery(),
            cancellationToken);

        return Ok(response);
    }

    [HttpGet("claims/statistics")]
    public async Task<IActionResult> GetClaimsStatistics(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetClaimsStatisticsQuery(),
            cancellationToken);

        return Ok(response);
    }
}
