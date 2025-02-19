using Workshop.WebApi.Authentication.Infrastructure.DI;
using Workshop.WebApi.Authentication.Infrastructure.Extensions;
using Workshop.WebApi.Authentication.Infrastructure;

namespace Workshop.WebApi.Authentication;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.RegisterConfigurations(builder.Configuration);
        
        builder.Services.AddCustomAuthentication(builder.Configuration);
        builder.Services.AddCustomAuthorization();

        builder.Services.AddControllers();
        builder.Services.AddDataProtection();

        builder.Services.AddDataSource(builder.Configuration);
        
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