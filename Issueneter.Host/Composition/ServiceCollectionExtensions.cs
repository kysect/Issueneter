namespace Issueneter.Host.Composition;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection GetRequiredOptions(this IServiceCollection services)
        => services;
}