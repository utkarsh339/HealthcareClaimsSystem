using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareClaims.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok("Public endpoint");
    }

    [Authorize]
    [HttpGet("authenticated")]
    public IActionResult Authenticated()
    {
        return Ok("Authenticated user");
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("admin")]
    public IActionResult Admin()
    {
        return Ok("Welcome Admin");
    }

    [Authorize(Policy = "ClaimsProcessorOnly")]
    [HttpGet("claims")]
    public IActionResult ClaimsProcessor()
    {
        return Ok("Welcome Claims Processor");
    }

    [Authorize(Policy = "MedicalReviewerOnly")]
    [HttpGet("review")]
    public IActionResult MedicalReviewer()
    {
        return Ok("Welcome Medical Reviewer");
    }

    [Authorize(Policy = "ProviderStaffOnly")]
    [HttpGet("provider")]
    public IActionResult ProviderStaff()
    {
        return Ok("Welcome Provider Staff");
    }
}