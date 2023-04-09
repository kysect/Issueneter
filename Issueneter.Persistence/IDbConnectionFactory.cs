using System.Data;
using Npgsql;
using Microsoft.Extensions.Options;

namespace Issueneter.Persistence;

public interface IDbConnectionFactory
{
    IDbConnection GetConnection();
}

public class DatabaseOptions
{
    public string ConnectionString { get; init; }
}

public class DefaultDbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DefaultDbConnectionFactory(IOptions<DatabaseOptions> dbOptions)
    {
        _connectionString = dbOptions.Value.ConnectionString;
    }

    public IDbConnection GetConnection() => new NpgsqlConnection(_connectionString);
}