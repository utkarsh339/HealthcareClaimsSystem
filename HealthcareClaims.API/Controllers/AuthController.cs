using HealthcareClaims.Application.Authentication.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareClaims.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
    [FromBody] RegisterUserCommand command)
    {
        var response = await _mediator.Send(command);

        return Ok(response);
    }
}