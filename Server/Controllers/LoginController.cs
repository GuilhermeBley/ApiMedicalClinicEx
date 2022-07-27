using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiMedicalClinicEx.Server.Controllers;

[ApiController, Route("api/[controller]")]
public class LoginController : ControllerBase
{
    [HttpGet, Authorize]
    public async Task<IActionResult> Get()
    {
        await Task.CompletedTask;
        return Ok();
    }
}