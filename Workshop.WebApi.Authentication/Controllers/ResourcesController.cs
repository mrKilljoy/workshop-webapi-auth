using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workshop.WebApi.Authentication.Infrastructure;

namespace Workshop.WebApi.Authentication.Controllers;

/// <summary>
/// A set of API providing access to various resources.
/// </summary>
[Route("api/[controller]")]
public class ResourcesController : ControllerBase
{
    /// <summary>
    /// Provide a client with the public resource.
    /// </summary>
    /// <remarks>This API is available for all clients.</remarks>
    /// <returns>A plaintext response.</returns>
    /// <response code="200">Access granted.</response>
    [AllowAnonymous]
    [HttpGet("public")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> GetPublicResource()
    {
        return Task.FromResult<IActionResult>(Ok("Here is the public resource"));
    }

    /// <summary>
    /// Provide a client with protected resource #1.
    /// </summary>
    /// <remarks>This API uses cookie-based authentication.</remarks>
    /// <response code="200">Access granted.</response>
    /// <response code="401">Access denied.</response>
    [Authorize(AuthenticationSchemes = Constants.Authentication.CookieSchemaName)]
    [HttpGet("protected-1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    public Task<IActionResult> GetProtectedResourceOne()
    {
        return Task.FromResult((IActionResult)Ok("Here is the protected resource #1"));
    }
    
    /// <summary>
    /// Provide a client with protected resource #2.
    /// </summary>
    /// <remarks>This API uses JWT bearer-based authentication.</remarks>
    /// <response code="200">Access granted.</response>
    /// <response code="401">Access denied.</response>
    [Authorize(AuthenticationSchemes = Constants.Authentication.JwtSchemaName, Policy = Constants.Authentication.PolicyName)]
    [HttpGet("protected-2")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    public Task<IActionResult> GetProtectedResourceTwo()
    {
        return Task.FromResult((IActionResult)Ok("Here is the protected resource #2"));
    }
    
    /// <summary>
    /// Provide a client with protected resource #3.
    /// </summary>
    /// <remarks>This API uses JWT bearer and cookie-based authentication schemas.</remarks>
    /// <response code="200">Access granted.</response>
    /// <response code="401">Access denied.</response>
    [Authorize(AuthenticationSchemes = $"{Constants.Authentication.CookieSchemaName},{Constants.Authentication.JwtSchemaName}")]
    [HttpGet("protected-3")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    public Task<IActionResult> GetProtectedResourceThree()
    {
        return Task.FromResult((IActionResult)Ok("Here is the protected resource #3"));
    }
}