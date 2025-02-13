using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Workshop.Shared.Services;
using Workshop.WebApi.Cookie.Infrastructure.Authentication;
using Workshop.WebApi.Cookie.Infrastructure.Configuration;

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
}