using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Workshop.Shared.Data;
using Workshop.WebApi.Auth.Infrastructure;
using Workshop.WebApi.Auth.Infrastructure.Authentication;
using Workshop.WebApi.Auth.Infrastructure.DI;
using Workshop.WebApi.Auth.Infrastructure.Extensions;

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

        builder.Services.AddDbContext<UserDbContext>(b => 
            b.UseInMemoryDatabase(Constants.Data.DatabaseName));

        builder.Services.RegisterConfigurations(builder.Configuration);
        builder.Services.RegisterDependencies();
        
        var app = builder.Build();
        
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();

        if (app.Configuration.IsTestDataNeeded())
        {
            app.Services.AddTestData();
        }

        app.Run();
    }
}