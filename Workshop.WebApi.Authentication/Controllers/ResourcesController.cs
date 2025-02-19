using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workshop.WebApi.Authentication.Infrastructure;

namespace Workshop.WebApi.Authentication.Controllers;

[Route("api/[controller]")]
public class ResourcesController : ControllerBase
{
    /// <summary>
    /// Provide a client with the public resource.
    /// </summary>
    /// <remarks>This API is available for all clients.</remarks>
    [HttpGet("public")]
    public Task<IActionResult> GetPublicResource()
    {
        return Task.FromResult<IActionResult>(Ok("Here is the public resource"));
    }

    /// <summary>
    /// Provide a client with protected resource #1.
    /// </summary>
    /// <remarks>This API uses cookie-based authentication.</remarks>
    [Authorize(AuthenticationSchemes = Constants.Authentication.CookieSchemaName)]
    [HttpGet("protected-1")]
    public Task<IActionResult> GetProtectedResourceOne()
    {
        return Task.FromResult((IActionResult)Ok("Here is the protected resource #1"));
    }
    
    /// <summary>
    /// Provide a client with protected resource #2.
    /// </summary>
    /// <remarks>This API uses JWT bearer-based authentication.</remarks>
    [Authorize(AuthenticationSchemes = Constants.Authentication.JwtSchemaName, Policy = Constants.Authentication.PolicyName)]
    [HttpGet("protected-2")]
    public Task<IActionResult> GetProtectedResourceTwo()
    {
        return Task.FromResult((IActionResult)Ok("Here is the protected resource #2"));
    }
    
    /// <summary>
    /// Provide a client with protected resource #3.
    /// </summary>
    /// <remarks>This API uses JWT bearer and cookie-based authentication schemas.</remarks>
    [Authorize(AuthenticationSchemes = $"{Constants.Authentication.CookieSchemaName},{Constants.Authentication.JwtSchemaName}")]
    [HttpGet("protected-3")]
    public Task<IActionResult> GetProtectedResourceThree()
    {
        return Task.FromResult((IActionResult)Ok("Here is the protected resource #3"));
    }
}