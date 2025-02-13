using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace Workshop.WebApi.Cookie.Infrastructure.Authentication;

public class CustomJwtBearerAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public TokenValidationParameters ValidationParameters { get; set; }
}