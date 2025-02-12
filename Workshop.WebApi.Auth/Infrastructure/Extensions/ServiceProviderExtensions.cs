using Workshop.Shared.Data;

namespace Workshop.WebApi.Auth.Infrastructure.Extensions;

public static class ServiceProviderExtensions
{
    public static void AddTestData(this IServiceProvider serviceProvider)
    {
        using var context = serviceProvider.GetRequiredService<UserDbContext>();
        context.AddTestData();
    }
}