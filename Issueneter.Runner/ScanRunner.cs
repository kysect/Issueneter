using Hangfire;
using Issueneter.Domain;
using Issueneter.Domain.Models;
using Issueneter.Filters;
using Issueneter.Github;
using Issueneter.Persistence;
using Issueneter.Telegram;
using Newtonsoft.Json;

namespace Issueneter.Runner;

public class ScanRunner
{
    private readonly TelegramSender _sender;
    private readonly GithubApiService _github;
    private readonly ScanStorage _storage;
    
    public ScanRunner(TelegramSender sender, GithubApiService github, ScanStorage storage)
    {
        _sender = sender;
        _github = github;
        _storage = storage;
    }

    public async Task Run(long scanId)
    {
        var scan = await _storage.GetScan(scanId);

        var source = new ActivitySource(scan.Owner, scan.Repo);
        IReadOnlyCollection<IFilterable> results;
        
        if (scan.ScanType == 2)
        {
            var issues = await _github.GetIssues(DateTimeOffset.Now - TimeSpan.FromMinutes(3), source);
            var rootFilter = JsonConvert.DeserializeObject<IFilter<Issue>>(scan.Filters);
            results = issues.Where(k => rootFilter.Apply(k)).ToArray();

        }
        else
        {
            var issues = await _github.GetPullRequests(DateTimeOffset.Now - TimeSpan.FromMinutes(3), source);
            var rootFilter = JsonConvert.DeserializeObject<IFilter<PullRequest>>(scan.Filters);
            results = issues.Where(k => rootFilter.Apply(k)).ToArray();
        }
        
        await _sender.SendResults(0, results);
    }
}