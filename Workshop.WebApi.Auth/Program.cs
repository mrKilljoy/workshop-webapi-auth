using Microsoft.AspNetCore.Authentication;
using Workshop.WebApi.Auth.Infrastructure;
using Workshop.WebApi.Auth.Infrastructure.Authentication;

namespace Workshop.WebApi.Auth;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddAuthentication(Constants.Authentication.Cookie)
            .AddScheme<AuthenticationSchemeOptions, CustomCookieAuthenticationHandler>(
                Constants.Authentication.Cookie,
                null);
        builder.Services.AddAuthorization();

        builder.Services.AddControllers();
        builder.Services.AddDataProtection();
        
        var app = builder.Build();
        
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}