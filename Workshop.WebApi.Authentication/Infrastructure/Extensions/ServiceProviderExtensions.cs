using Workshop.Shared.Data;
using Workshop.Shared.Extensions;

namespace Workshop.WebApi.Authentication.Infrastructure.Extensions;

public static class ServiceProviderExtensions
{
    public static async Task AddTestData(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        await context.AddTestData();
    }
}