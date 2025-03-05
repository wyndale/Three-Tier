using Microsoft.EntityFrameworkCore;
using server.Data;

namespace server.Extensions;

public static class DatabaseServiceExtension
{
    public static IServiceCollection AddDatabaseService(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStrings = configuration.GetConnectionString("ClothingStoreConnection")
            ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ClothingStoreDbContext>(options =>
        {
            options.UseSqlServer(connectionStrings);
        });

        return services;
    }
}
