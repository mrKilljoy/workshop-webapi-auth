using Workshop.WebApi.Authentication.Infrastructure.DI;
using Workshop.WebApi.Authentication.Infrastructure.Extensions;
using Workshop.WebApi.Authentication.Infrastructure.Filters;

namespace Workshop.WebApi.Authentication;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.RegisterConfigurations(builder.Configuration);
        
        builder.Services.AddCustomAuthentication(builder.Configuration);
        builder.Services.AddCustomAuthorization();

        builder.Services.AddControllers(x =>
        {
            x.Filters.Add<CustomGlobalExceptionFilter>();
        });
        
        builder.Services.AddDataProtection();

        builder.Services.AddDataSource(builder.Configuration);
        
        builder.Services.RegisterDependencies();

        builder.Services.AddSwaggerSupport();
        
        var app = builder.Build();
        
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.MapControllers();

        if (app.Configuration.UseTestData())
        {
            await app.Services.AddTestData();
        }

        await app.RunAsync();
    }
}