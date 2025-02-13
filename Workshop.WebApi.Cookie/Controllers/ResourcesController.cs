using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workshop.WebApi.Cookie.Infrastructure;

namespace Workshop.WebApi.Cookie.Controllers;

[Route("api/[controller]")]
public class ResourcesController : ControllerBase
{
    [HttpGet("public")]
    public async Task<IActionResult> GetPublicResource()
    {
        return Ok("Here is the public resource");
    }

    [Authorize(AuthenticationSchemes = Constants.Authentication.CookieSchemaName)]
    [HttpGet("protected-1")]
    public Task<IActionResult> GetProtectedResourceOne()
    {
        return Task.FromResult((IActionResult)Ok("Here is the protected resource #1"));
    }
    
    [Authorize(AuthenticationSchemes = Constants.Authentication.JwtSchemaName)]
    [HttpGet("protected-2")]
    public Task<IActionResult> GetProtectedResourceTwo()
    {
        return Task.FromResult((IActionResult)Ok("Here is the protected resource #2"));
    }
}