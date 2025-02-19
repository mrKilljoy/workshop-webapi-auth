using Workshop.WebApi.Authentication.Infrastructure.Configuration;

namespace Workshop.WebApi.Authentication.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static bool UseTestData(this IConfiguration configuration)
    {
        var dataSourceSection = configuration?.GetSection(DataSourceOptions.SectionName)?.Get<DataSourceOptions>();
        return dataSourceSection?.UseTestData ?? false;
    }
}