using HealthcareClaims.API.Models.Providers;
using HealthcareClaims.Application.Providers.Common;
using HealthcareClaims.Application.Providers.CreateProvider;
using HealthcareClaims.Application.Providers.DeleteProvider;
using HealthcareClaims.Application.Providers.GetAllProviders;
using HealthcareClaims.Application.Providers.GetProviderById;
using HealthcareClaims.Application.Providers.UpdateProvider;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareClaims.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProvidersController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProvidersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Policy = "ProviderWriteAccess")]
    [ProducesResponseType(typeof(ProviderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateProvider(
        CreateProviderRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateProviderCommand(
            request.ProviderName,
            request.ProviderType,
            request.NPI,
            request.Email,
            request.PhoneNumber,
            request.Address,
            request.City,
            request.State,
            request.ZipCode);

        var response = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetProviderById),
            new { id = response.Id },
            response);
    }

    [HttpGet("{id:guid}", Name = nameof(GetProviderById))]
    [Authorize(Policy = "ProviderReadAccess")]
    [ProducesResponseType(typeof(ProviderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProviderResponse>> GetProviderById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var provider = await _mediator.Send(
            new GetProviderByIdQuery(id),
            cancellationToken);

        if (provider is null)
        {
            return NotFound();
        }

        return Ok(provider);
    }

    [HttpGet]
    [Authorize(Policy = "ProviderReadAccess")]
    [ProducesResponseType(typeof(IReadOnlyList<ProviderResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ProviderResponse>>> GetAllProviders(
        CancellationToken cancellationToken)
    {
        var providers = await _mediator.Send(
            new GetAllProvidersQuery(),
            cancellationToken);

        return Ok(providers);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "ProviderWriteAccess")]
    [ProducesResponseType(typeof(ProviderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProviderResponse>> UpdateProvider(
        Guid id,
        UpdateProviderRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateProviderCommand(
            id,
            request.ProviderName,
            request.ProviderType,
            request.NPI,
            request.Email,
            request.PhoneNumber,
            request.Address,
            request.City,
            request.State,
            request.ZipCode,
            request.IsActive);

        var provider = await _mediator.Send(command, cancellationToken);

        return Ok(provider);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "ProviderWriteAccess")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteProvider(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteProviderCommand(id),
            cancellationToken);

        return NoContent();
    }
}