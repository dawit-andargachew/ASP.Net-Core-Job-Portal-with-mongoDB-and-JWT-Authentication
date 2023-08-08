using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[Controller]")]
public class TestController : ControllerBase
{
    [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Test()
    {
        return Ok("testing");
    }
}
