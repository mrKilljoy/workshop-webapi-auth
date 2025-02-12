using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workshop.WebApi.Auth.Cookie.Infrastructure;

namespace Workshop.WebApi.Auth.Cookie.Controllers;

[Route("api/[controller]")]
public class ResourcesController : ControllerBase
{
    [HttpGet("public")]
    public async Task<IActionResult> GetPublicResource()
    {
        return Ok("Here is the public resource");
    }

    [Authorize(AuthenticationSchemes = Constants.Authentication.Cookie)]
    [HttpGet("protected")]
    public async Task<IActionResult> GetProtectedResource()
    {
        return Ok("Here is the protected resource");
    }
}