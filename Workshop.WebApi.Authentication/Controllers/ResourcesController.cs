using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workshop.WebApi.Authentication.Infrastructure;
using Workshop.WebApi.Authentication.Models;

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
    [ProducesResponseType(typeof(ResourceResponse), StatusCodes.Status200OK, "application/json")]
    public Task<IActionResult> GetPublicResource()
    {
        return Task.FromResult<IActionResult>(Ok(BuildResponse("public", "Here is the public resource")));
    }

    /// <summary>
    /// Provide a client with protected resource #1.
    /// </summary>
    /// <remarks>This API uses cookie-based authentication.</remarks>
    /// <response code="200">Access granted.</response>
    /// <response code="401">Access denied.</response>
    [Authorize(AuthenticationSchemes = Constants.Authentication.CookieSchemaName)]
    [ProducesResponseType(typeof(ResourceResponse), StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized, "application/json")]
    [HttpGet("protected-1")]
    public Task<IActionResult> GetProtectedResourceOne()
    {
        return Task.FromResult((IActionResult)Ok(BuildResponse("protected-1", "Here is the protected resource #1")));
    }
    
    /// <summary>
    /// Provide a client with protected resource #2.
    /// </summary>
    /// <remarks>This API uses JWT bearer-based authentication.</remarks>
    /// <response code="200">Access granted.</response>
    /// <response code="401">Access denied.</response>
    [Authorize(AuthenticationSchemes = Constants.Authentication.JwtSchemaName, Policy = Constants.Authentication.PolicyName)]
    [ProducesResponseType(typeof(ResourceResponse), StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized, "application/json")]
    [HttpGet("protected-2")]
    public Task<IActionResult> GetProtectedResourceTwo()
    {
        return Task.FromResult((IActionResult)Ok(BuildResponse("protected-2", "Here is the protected resource #2")));
    }
    
    /// <summary>
    /// Provide a client with protected resource #3.
    /// </summary>
    /// <remarks>This API uses JWT bearer and cookie-based authentication schemas.</remarks>
    /// <response code="200">Access granted.</response>
    /// <response code="401">Access denied.</response>
    [Authorize(AuthenticationSchemes = $"{Constants.Authentication.CookieSchemaName},{Constants.Authentication.JwtSchemaName}")]
    [ProducesResponseType(typeof(ResourceResponse), StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized, "application/json")]
    [HttpGet("protected-3")]
    public Task<IActionResult> GetProtectedResourceThree()
    {
        return Task.FromResult((IActionResult)Ok(BuildResponse("protected-3", "Here is the protected resource #3")));
    }

    private ResourceResponse BuildResponse(string name, string content, int httpCode = 200)
        => new() { Name = name, Content = content, HttpCode = httpCode};
}