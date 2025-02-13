using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Workshop.Shared.Data;
using Workshop.Shared.Services;
using Workshop.WebApi.Cookie.Infrastructure.Authentication;
using Workshop.WebApi.Cookie.Infrastructure.Authentication.Handlers;
using Workshop.WebApi.Cookie.Infrastructure.Configuration;
using Workshop.WebApi.Cookie.Infrastructure.Exceptions;

namespace Workshop.WebApi.Cookie.Infrastructure.DI;

public static class DependencyRegistration
{
    public static IServiceCollection RegisterDependencies(this IServiceCollection services)
    {
        services.AddTransient<IUserManager, UserManager>();
        
        return services;
    }
    
    public static IServiceCollection RegisterConfigurations(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.Configure<DataSource>(configuration.GetSection(Constants.Configuration.DataSourceSection));
        serviceCollection.Configure<Security>(configuration.GetSection(Constants.Configuration.SecuritySection));

        return serviceCollection;
    }

    /// <summary>
    /// Register multiple authentication schemes.
    /// </summary>
    public static IServiceCollection AddCustomAuthentication(
        this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        // private key is kept in app configuration for testing puropses
        var encKey = configuration?.GetSection(Constants.Configuration.SecuritySection)?.Get<Security>();

        var authBuilder = serviceCollection
            .AddAuthentication();
        
        authBuilder.AddScheme<AuthenticationSchemeOptions, CustomCookieAuthenticationHandler>(
                Constants.Authentication.CookieSchemaName,
                null);
        
        if (string.IsNullOrEmpty(encKey?.EncryptionKey))
        {
            Debug.WriteLine("No encryption key is provided, JWT authentication is disabled");
            return serviceCollection;
        }

        authBuilder.AddScheme<CustomJwtBearerAuthenticationSchemeOptions, CustomJwtBearerAuthenticationHandler>(
            Constants.Authentication.JwtSchemaName,
            options =>
            {
                options.ValidationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidIssuer = Constants.Authentication.JwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = Constants.Authentication.JwtAudience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encKey.EncryptionKey)),
                };
            });
        
        return serviceCollection;
    }

    public static IServiceCollection AddDataSource(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var dataSourceSettings = configuration.GetSection(Constants.Configuration.DataSourceSection)?.Get<DataSource>();
        if (dataSourceSettings is null)
        {
            throw new MissingDataSourceException();
        }

        switch (dataSourceSettings.Type)
        {
            case DataSourceType.InMemory:
                serviceCollection.AddDbContext<UserDbContext>(b =>
                    b.UseInMemoryDatabase(Constants.Data.DatabaseName));
                break;
            case DataSourceType.MSSQL:
                serviceCollection.AddDbContext<UserDbContext>(b =>
                    b.UseSqlServer(dataSourceSettings.ConnectionString));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dataSourceSettings.Type));
        }
        
        return serviceCollection;
    }
}