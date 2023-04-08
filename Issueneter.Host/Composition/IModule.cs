using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Issueneter.Host.Composition;

public interface IModule
{
    static abstract IServiceCollection Add(IServiceCollection services, IHostEnvironment env, IConfigurationRoot configuration);
    static abstract WebApplication Use(WebApplication applicationBuilder);
}

public static class Extensions
{
    public static IServiceCollection AddModule<T>(this IServiceCollection services, IHostEnvironment env,
        IConfigurationRoot configuration)
        where T : IModule
        => T.Add(services, env, configuration);
    
    public static WebApplication UseModule<T>(this WebApplication app)
        where T : IModule
        => T.Use(app);
}