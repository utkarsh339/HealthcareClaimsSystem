using HealthcareClaims.API.Models.Payments;
using HealthcareClaims.Application.Payments.CreatePayment;
using HealthcareClaims.Application.Payments.DeletePayment;
using HealthcareClaims.Application.Payments.GetAllPayments;
using HealthcareClaims.Application.Payments.GetPaymentById;
using HealthcareClaims.Application.Payments.UpdatePayment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareClaims.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Policy = "PaymentWriteAccess")]
    public async Task<IActionResult> Create(
        CreatePaymentRequest request)
    {
        var result = await _mediator.Send(
            new CreatePaymentCommand(
                request.ClaimId,
                request.Amount,
                request.PaymentMethod));

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "PaymentReadAccess")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(
            new GetPaymentByIdQuery(id));

        return Ok(result);
    }

    [HttpGet]
    [Authorize(Policy = "PaymentReadAccess")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(
            new GetAllPaymentsQuery());

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "PaymentWriteAccess")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdatePaymentRequest request)
    {
        var result = await _mediator.Send(
            new UpdatePaymentCommand(
                id,
                request.Amount,
                request.PaymentMethod,
                request.Status,
                request.PaidDate));

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "PaymentWriteAccess")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(
            new DeletePaymentCommand(id));

        return NoContent();
    }
}