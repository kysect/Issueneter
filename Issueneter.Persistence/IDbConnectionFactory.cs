using System.Data;
using Npgsql;
using Microsoft.Extensions.Options;

namespace Issueneter.Persistence;

public interface IDbConnectionFactory
{
    IDbConnection GetConnection();
}