using Microsoft.EntityFrameworkCore;
using Workshop.Shared.Data;
using Workshop.WebApi.Cookie.Infrastructure.DI;
using Workshop.WebApi.Cookie.Infrastructure.Extensions;
using Workshop.WebApi.Cookie.Infrastructure;

namespace Workshop.WebApi.Cookie;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.RegisterConfigurations(builder.Configuration);
        
        builder.Services.AddCustomAuthentication(builder.Configuration);
        builder.Services.AddAuthorization();

        builder.Services.AddControllers();
        builder.Services.AddDataProtection();

        builder.Services.AddDbContext<UserDbContext>(b => 
            b.UseInMemoryDatabase(Constants.Data.DatabaseName));
        
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