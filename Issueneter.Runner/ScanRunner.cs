using Issueneter.Domain.Models;
using Issueneter.Github;
using Issueneter.Json;
using Issueneter.Mappings;
using Issueneter.Persistence;
using Issueneter.Telegram;
using Newtonsoft.Json;

namespace Issueneter.Runner;

public class ScanRunner
{
    private readonly TelegramSender _sender;
    private readonly GithubApiService _github;
    private readonly ScanStorage _storage;
    private readonly IMessageFormatter<Issue> _issueFormatter;
    private readonly IMessageFormatter<PullRequest> _pullRequestFormatter;

    public ScanRunner(TelegramSender sender, GithubApiService github, ScanStorage storage, IMessageFormatter<Issue> issueMessageIssueFormatter, IMessageFormatter<PullRequest> pullRequestMessageFormatter)
    {
        _sender = sender;
        _github = github;
        _storage = storage;
        _issueFormatter = issueMessageIssueFormatter;
        _pullRequestFormatter = pullRequestMessageFormatter;
    }

    public async Task Run(long scanId)
    {
        var scan = await _storage.GetScan(scanId);
        
        if (scan is null)
            return;
        
        var source = new ActivitySource(scan.Owner, scan.Repo);
        if (scan.ScanType == ScanType.Issue)
        {
            var issues = await _github.GetIssues(DateTimeOffset.Now - TimeSpan.FromMinutes(45), source);

            var filters = JsonConvert.DeserializeObject<String>(scan.Filters);
            var rootFilter = IssueneterJsonSerializer.Deserialize<Issue>(filters);
            var results = issues.Where(k => rootFilter.Apply(k)).ToArray();
            await _sender.SendResults(scan.ChatId, _issueFormatter, results);
        }
        else
        {
            var issues = await _github.GetPullRequests(DateTimeOffset.Now - TimeSpan.FromMinutes(30), source);
            var rootFilter = IssueneterJsonSerializer.Deserialize<PullRequest>(scan.Filters);
            var results = issues.Where(k => rootFilter.Apply(k)).ToArray();
            await _sender.SendResults(scan.ChatId, _pullRequestFormatter, results);
        }
    }
}