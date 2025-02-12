using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Workshop.Shared.Data;
using Workshop.WebApi.Auth.Cookie.Infrastructure;
using Workshop.WebApi.Auth.Cookie.Infrastructure.Authentication;
using Workshop.WebApi.Auth.Cookie.Infrastructure.DI;
using Workshop.WebApi.Auth.Cookie.Infrastructure.Extensions;

namespace Workshop.WebApi.Auth.Cookie;

public class Program
{
    public static async Task Main(string[] args)
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

        builder.Services.AddDbContext<UserDbContext>(b => 
            b.UseInMemoryDatabase(Constants.Data.DatabaseName));

        builder.Services.RegisterConfigurations(builder.Configuration);
        builder.Services.RegisterDependencies();
        
        var app = builder.Build();
        
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();

        if (app.Configuration.UseTestData())
        {
            await app.Services.AddTestData();
        }

        await app.RunAsync();
    }
}