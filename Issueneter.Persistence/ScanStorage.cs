using Dapper;

namespace Issueneter.Persistence;

public class ScanStorage
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ScanStorage(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyCollection<int>> GetAllScansIds()
    {
        const string query = @"SELECT id FROM issueneter.scans";

        using var connection = _connectionFactory.GetConnection();
        return (await connection.QueryAsync<int>(query)).AsList();
    }
    public async Task<ScanEntry?> GetScan(long scanId)
    {
        const string query = @"
            SELECT id, scan_type as Type, owner, repo, filters FROM issueneter.scans
            WHERE id = @id";

        var @params = new {id = scanId}; 
        using var connection = _connectionFactory.GetConnection();
        return await connection.QuerySingleOrDefaultAsync<ScanEntry>(query, @params);
    }

    public async Task<long> CreateNewScan(ScanCreation creation)
    {
        const string query = @"
            INSERT INTO issueneter.scans
            (scan_type, owner, repo, created, filters)
            VALUES (@type, @acc, @repo, now(), to_json(@filters))
            RETURNING id";

        var @params = new
        {
            type = (int)creation.Type,
            acc = creation.Owner,
            repo = creation.Repo,
            filters = creation.Filters
        };
        
        var connection = _connectionFactory.GetConnection();
        
        return await connection.ExecuteScalarAsync<int>(query, @params);
    }
}