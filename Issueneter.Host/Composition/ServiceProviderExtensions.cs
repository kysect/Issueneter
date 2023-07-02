using Microsoft.Extensions.Options;

namespace Issueneter.Host.Composition;

public static class ServiceProviderExtensions
{
    public static T GetRequiredOptions<T>(this IServiceProvider sp) where T : class 
        => sp.GetRequiredService<IOptions<T>>().Value;
}