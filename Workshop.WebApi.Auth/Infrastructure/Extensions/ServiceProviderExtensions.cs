using Workshop.Shared.Data;

namespace Workshop.WebApi.Auth.Infrastructure.Extensions;

public static class ServiceProviderExtensions
{
    public static async Task AddTestData(this IServiceProvider serviceProvider)
    {
        using var context = serviceProvider.GetRequiredService<UserDbContext>();
        await context.AddTestData();
    }
}