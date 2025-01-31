using CureFusion.Persistence;

namespace CureFusion;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies (this IServiceCollection services,IConfiguration config)
    {


        services.AddDatabaseConnection (config);
        services.AddHttpContextAccessor();
        return services;

    }


    private static IServiceCollection AddDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection String Not Found");
         services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(connectionString));

        return services;
    }
}
