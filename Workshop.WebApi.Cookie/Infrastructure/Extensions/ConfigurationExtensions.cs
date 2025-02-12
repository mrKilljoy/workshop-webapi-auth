using Workshop.WebApi.Cookie.Infrastructure.Configuration;

namespace Workshop.WebApi.Cookie.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static bool UseTestData(this IConfiguration configuration)
    {
        var dataSourceSection = configuration?.GetSection(Constants.Configuration.DataSourceSection)?.Get<DataSource>();
        return dataSourceSection?.UseTestData ?? false;
    }
}