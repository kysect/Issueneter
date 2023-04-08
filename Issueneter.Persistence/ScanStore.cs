using Dapper;
using Issueneter.ApiModels.Requests;
using Issueneter.ApiModels.Responses;

namespace Issueneter.Persistence;

public class ScanStore
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ScanStore(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyCollection<int>> GetAllScansIds()
    {
        const string query = @"SELECT id FROM issueneter.scans";

        using var connection = _connectionFactory.GetConnection();
        return (await connection.QueryAsync<int>(query)).AsList();
    }
    public async Task<ScanResponse?> GetScan(int scanId)
    {
        const string query = @"
            SELECT id, scan_type as Type, owner, repo, filters FROM issueneter.scans
            WHERE id = @id";

        var @params = new {id = scanId}; 
        using var connection = _connectionFactory.GetConnection();
        return await connection.QuerySingleOrDefaultAsync<ScanResponse>(query, @params);
    }

    public Task CreateNewScan(ScanCreation creation)
    {
        const string query = @"
            INSERT INTO issueneter.scans
            (scan_type, owner, repo, created, filters)
            VALUES (@type, @acc, @repo, now(), to_json(@filters))";

        var @params = new
        {
            type = (int)creation.Type,
            acc = creation.Owner,
            repo = creation.Repo,
            filters = creation.Filters
        };
        
        var connection = _connectionFactory.GetConnection();

        return connection.ExecuteAsync(query, @params);
    }
}