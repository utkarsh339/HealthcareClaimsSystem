using Microsoft.AspNetCore.Mvc;

namespace HealthcareClaims.API.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        throw new Exception("Testing middleware");
    }
}