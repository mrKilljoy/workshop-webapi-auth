using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace Workshop.WebApi.Authentication.Infrastructure.Authentication.Handlers;

public class CustomJwtBearerAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public TokenValidationParameters ValidationParameters { get; set; }
}