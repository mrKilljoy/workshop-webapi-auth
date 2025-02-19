using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Workshop.WebApi.Authentication.Infrastructure.Authentication.Handlers;

public class CustomJwtBearerAuthenticationHandler : AuthenticationHandler<CustomJwtBearerAuthenticationSchemeOptions>
{
    public CustomJwtBearerAuthenticationHandler(IOptionsMonitor<CustomJwtBearerAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Authorization header is missing");
        }

        string authorization = Request.Headers.Authorization;
        if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer "))
        {
            return AuthenticateResult.Fail("No valid token provided");
        }

        var token = authorization.Substring("Bearer ".Length).Trim();
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var validationResult = await tokenHandler.ValidateTokenAsync(token, Options.ValidationParameters);

            var principal = new ClaimsPrincipal(validationResult.ClaimsIdentity);
            
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
        catch (Exception e)
        {
            return AuthenticateResult.Fail($"Token validation failed: {e.Message}");
        }
    }
}