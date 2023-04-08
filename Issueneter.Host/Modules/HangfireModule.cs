using Hangfire;
using Hangfire.PostgreSql;
using Issueneter.Host.Composition;
using Issueneter.Host.Options;

namespace Issueneter.Host.Modules;

public static class HangfireModule
{
    public static IServiceCollection AddHangfireModule(this IServiceCollection services, IHostEnvironment env, IConfigurationRoot configuration)
        => services
            .AddHangfire((sp, config) =>
            {
                config
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UsePostgreSqlStorage(sp.GetRequiredOptions<DatabaseOptions>().ConnectionString);
            })
            .AddHangfireServer();

    public static WebApplication UseHangfireModule(this WebApplication app)
    {
        app.MapHangfireDashboard();
        app.UseHangfireDashboard();

        return app;
    }
}