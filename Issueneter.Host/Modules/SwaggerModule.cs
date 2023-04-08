using Issueneter.Host.Composition;

namespace Issueneter.Host.Modules;

public static class SwaggerModule
{
    public static IServiceCollection AddSwaggerModule(this IServiceCollection services, IHostEnvironment env,
        IConfigurationRoot configuration)
        => services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

    public static WebApplication UseSwaggerModule(this WebApplication app)
    {
        app
            .UseSwagger()
            .UseSwaggerUI();

        return app;
    }
}