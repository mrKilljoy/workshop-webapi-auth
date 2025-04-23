using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Workshop.Shared.Services;
using Workshop.WebApi.Authentication.Models;

namespace Workshop.WebApi.Authentication.Infrastructure.Authentication.Handlers;

public class CustomCookieAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IUserManager _userManager;
    private readonly IDataProtector dataProtector;

    public CustomCookieAuthenticationHandler(IDataProtectionProvider idp,
        IUserManager userManager,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) :
        base(options, logger, encoder)
    {
        _userManager = userManager;
        dataProtector = idp.CreateProtector(Constants.Authentication.DataProtectorName);
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            if (!Request.Cookies.TryGetValue(Constants.Authentication.CookieSchemaName, out var encryptedCookie))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return AuthenticateResult.Fail("Required cookie not found");
            }

            var cookieValue = DecryptCookie(encryptedCookie);
        
            if (!await VerifyUserPresence(cookieValue))
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return AuthenticateResult.Fail("Username not found");
            }

            return await Task.FromResult(AuthenticateResult.Success(BuildTicket(cookieValue)));
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Unexpected authentication error (cookies)");
            return AuthenticateResult.NoResult();
        }
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        var errorResponse = new ErrorResponse()
        {
            Error = "Authentication failed",
            HttpCode = Response.StatusCode
        };

        return Response.WriteAsJsonAsync(errorResponse);
    }

    private string DecryptCookie(string protectedCookie)
    {
        string cookieValue;
        try
        {
            cookieValue = dataProtector.Unprotect(protectedCookie);
        }
        catch
        {
            // any exception should be logged, right?
            cookieValue = null;
        }

        return cookieValue;
    }
    
    private AuthenticationTicket BuildTicket(string login)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, login)
        };
        
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        return new AuthenticationTicket(principal, Scheme.Name);
    }
    
    private async Task<bool> VerifyUserPresence(string login)
    {
        return await _userManager.Get(login) is not null;
    }
}