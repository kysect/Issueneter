using Hangfire;
using Hangfire.PostgreSql;
using Issueneter.Host.Options;
using Issueneter.Host.TempDirecory;
using Issueneter.Persistence;

namespace Issueneter.Host.Composition;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbRelated(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddSingleton<IDbConnectionFactory, DbConnectionFactory>()
            .Configure<DatabaseOptions>(configuration.GetSection(nameof(DatabaseOptions)))
            .AddSingleton<ScanStore>();

    public static IServiceCollection AddHangfireRelated(this IServiceCollection services)
        => services
            .AddHangfire((sp, config) =>
                {
                    config
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UsePostgreSqlStorage(sp.GetRequiredOptions<DatabaseOptions>().ConnectionString);
                })
            .AddHangfireServer();

    public static IServiceCollection AddSwaggerRelated(this IServiceCollection services)
        => services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();
}