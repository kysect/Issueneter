using Issueneter.Host.Composition;
using Issueneter.Persistence;

namespace Issueneter.Host.Modules;

public static class DatabaseModule
{
    public static IServiceCollection AddDatabaseModule(this IServiceCollection services, IHostEnvironment env,
        IConfigurationRoot configuration)
        => services
            .AddSingleton<IDbConnectionFactory, DefaultDbConnectionFactory>()
            .Configure<DatabaseOptions>(configuration.GetSection(nameof(DatabaseOptions)))
            .AddSingleton<ScanStorage>();
}