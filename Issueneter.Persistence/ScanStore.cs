using Dapper;
using Issueneter.ApiModels.Requests;

namespace Issueneter.Persistence;

public class ScanStore
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ScanStore(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task GetScan(int scanId)
    {
        const string query = @"
            SELECT * FROM issueneter.scans
            WHERE id = @id";

        var @params = new {id = scanId}; 
        using var connection = _connectionFactory.GetConnection();
        var result = await connection.QuerySingleOrDefaultAsync<int>(query, @params);
    }

    public async Task CreateNewScan(AddNewRepoScanRequest request)
    {
        if (request.Issues.IsSome)
        {
            var issueParams = new
            {
                type = "Issue",
                acc = request.AccountOrOrganization,
                repo = request.Repository,
                filters = request.Issues.Value
            };

            await CreateNewScan(issueParams);
        }

        if (request.PRs.IsSome)
        {
            var prParams = new
            {
                type = "Repo",
                acc = request.AccountOrOrganization,
                repo = request.Repository,
                filters = request.PRs.Value
            };
            
            await CreateNewScan(prParams);
        }
           
    }
    
    private Task CreateNewScan(object @params)
    {
        const string query = @"
            INSERT INTO issueneter.scans
            VALUES (@type, @acc, @repo, now(), @filters)";

        var connection = _connectionFactory.GetConnection();

        return connection.ExecuteAsync(query, @params);
    }
}