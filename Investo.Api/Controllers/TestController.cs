using Microsoft.AspNetCore.Mvc;

namespace Investo.Api.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class TestController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Get()
    {
        return this.Ok("Test");
    }
}