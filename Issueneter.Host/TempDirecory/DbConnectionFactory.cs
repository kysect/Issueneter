using System.Data;
using Issueneter.Host.Options;
using Issueneter.Persistence;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Issueneter.Host.TempDirecory;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IOptions<DatabaseOptions> dbOptions)
    {
        _connectionString = dbOptions.Value.ConnectionString;
    }

    public IDbConnection GetConnection() => new NpgsqlConnection(_connectionString);
}