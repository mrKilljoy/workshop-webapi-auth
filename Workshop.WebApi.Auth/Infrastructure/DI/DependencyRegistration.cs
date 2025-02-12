using Workshop.Shared.Services;
using Workshop.WebApi.Auth.Infrastructure.Configuration;

namespace Workshop.WebApi.Auth.Infrastructure.DI;

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

        return serviceCollection;
    }
}