using Microsoft.AspNetCore.Mvc;

namespace SGE.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "SGE WebApi is running" });
}
