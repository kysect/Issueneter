using Issueneter.Host.TempDirecory;
using Issueneter.Persistence;

namespace Issueneter.Host.Composition;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbConnectionFactory(this IServiceCollection services)
        => services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
}