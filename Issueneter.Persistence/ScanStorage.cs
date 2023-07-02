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
            SELECT id, scan_type as ScanType, owner, repo, chat_id AS ChatId, filters FROM issueneter.scans
            WHERE id = @id";

        var @params = new {id = scanId}; 
        using var connection = _connectionFactory.GetConnection();
        return await connection.QuerySingleOrDefaultAsync<ScanEntry>(query, @params);
    }

    public async Task<long> CreateNewScan(ScanCreation creation)
    {
        const string query = @"
            INSERT INTO issueneter.scans
            (scan_type, owner, repo, chat_id, created, filters)
            VALUES (@type, @acc, @repo, @chat, now(), to_json(@filters))
            RETURNING id";

        var @params = new
        {
            type = creation.Type,
            acc = creation.Owner,
            repo = creation.Repo,
            chat = creation.ChatId,
            filters = creation.Filters
        };
        
        var connection = _connectionFactory.GetConnection();
        
        return await connection.ExecuteScalarAsync<int>(query, @params);
    }
}