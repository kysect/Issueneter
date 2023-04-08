using Issueneter.Domain.Models;
using Issueneter.Domain.Utility;
using Issueneter.Github.Utility;
using Octokit;
using Issue = Issueneter.Domain.Models.Issue;
using PullRequest = Issueneter.Domain.Models.PullRequest;

namespace Issueneter.Github;

// TODO: Rename
public class GithubApiService
{
    private readonly IGitHubClient _client;
    
    public GithubApiService(IGitHubClient client)
    {
        _client = client;
    }

    public async Task<IReadOnlyCollection<Issue>> GetIssues(DateTimeOffset since, ActivitySource source)
    {
        var request = new RepositoryIssueRequest()
        {
            Since = since,
            SortProperty = IssueSort.Updated,
            SortDirection = SortDirection.Descending
        };
        
        var issues = await _client.Issue.GetAllForRepository(source.Owner, source.Repository, request);

        return issues
                .Where(i => i.PullRequest is null)
                .Select(i => new Issue(
                    i.Title,
                    i.User.Login,
                    i.HtmlUrl,
                    i.GetState(),
                    i.Labels.Select(l => l.Name).ToList(),
                    new Ref<List<TimelineEvent>>(() => GetActivity(since, source, i.Number))))
                .ToList();
    }
    
    public async Task<IReadOnlyCollection<PullRequest>> GetPullRequests(DateTimeOffset since, ActivitySource source)
    {
        var request = new PullRequestRequest()
        {
            SortProperty = PullRequestSort.Updated,
            SortDirection = SortDirection.Descending
        };
        
        var pullRequests = await _client.Repository.PullRequest.GetAllForRepository(source.Owner, source.Repository, request);

        return pullRequests
                .Select(pr => new PullRequest(
                    pr.Title,
                    pr.User.Login,
                    pr.HtmlUrl,
                    pr.GetState(),
                    pr.Labels.Select(l => l.Name).ToList(),
                    new Ref<List<TimelineEvent>>(() => GetActivity(since, source, pr.Number))))
                .ToList();
    }

    private async Task<List<TimelineEvent>> GetActivity(DateTimeOffset since, ActivitySource source, int elementNumber)
    {
        var activity = await _client.Issue.Timeline.GetAllForIssue(source.Owner, source.Repository, elementNumber);

        return activity.Where(a => a.CreatedAt < since).Select(a => a.ToTimelineEvent()).ToList();
    }
}