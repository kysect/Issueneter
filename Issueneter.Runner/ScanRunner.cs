using Issueneter.Domain;
using Issueneter.Domain.Models;
using Issueneter.Filters;
using Issueneter.Github;
using Issueneter.Json;
using Issueneter.Mappings;
using Issueneter.Persistence;
using Issueneter.Telegram;
using Issueneter.Telegram.Formatters;
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
        
        if (scan is null)
            return;
        
        var source = new ActivitySource(scan.Owner, scan.Repo);
        if (scan.ScanType == ScanType.Issue)
        {
            var formatter = new IssueMessageFormatter();
            var issues = await _github.GetIssues(DateTimeOffset.Now - TimeSpan.FromMinutes(45), source);

            var filters = JsonConvert.DeserializeObject<String>(scan.Filters);
            var rootFilter = IssueneterJsonSerializer.Deserialize<Issue>(filters);
            var results = issues.Where(k => rootFilter.Apply(k)).ToArray();
            await _sender.SendResults(scan.ChatId, formatter, results);
        }
        else
        {
            var issues = await _github.GetPullRequests(DateTimeOffset.Now - TimeSpan.FromMinutes(30), source);
            var rootFilter = IssueneterJsonSerializer.Deserialize<PullRequest>(scan.Filters);
            var results = issues.Where(k => rootFilter.Apply(k)).ToArray();
            var formatter = new PullRequestMessageFormatter();
            await _sender.SendResults(scan.ChatId, formatter, results);
        }
    }
}