using Issueneter.Host.Composition;
using Issueneter.Mappings;
using Issueneter.Persistence;
using Issueneter.Persistence.TypeHandlers;

namespace Issueneter.Host.Modules;

public static class DatabaseModule
{
    public static IServiceCollection AddDatabaseModule(this IServiceCollection services, IHostEnvironment env,
        IConfigurationRoot configuration)
    {
        Dapper.SqlMapper.AddTypeHandler(typeof(ScanType), new ScanTypeHandler());
        
        return services
            .AddSingleton<IDbConnectionFactory, DefaultDbConnectionFactory>()
            .Configure<DatabaseOptions>(configuration.GetSection(nameof(DatabaseOptions)))
            .AddSingleton<ScanStorage>();
    }
}