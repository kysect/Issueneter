using Issueneter.Domain.Models;
using Octokit;
using PullRequest = Octokit.PullRequest;

namespace Issueneter.Github.Utility;

public static class PullRequestExtension
{
    public static PullRequestState GetState(this PullRequest pullRequest) => pullRequest switch
    {
        { State.Value: ItemState.Open, Draft: true } => PullRequestState.Draft,
        { State.Value: ItemState.Open } => PullRequestState.Active,
        { State.Value: ItemState.Closed, Merged: true} => PullRequestState.Merged,
        { State.Value: ItemState.Closed } => PullRequestState.Aborted,
        _ => throw new ArgumentOutOfRangeException(nameof(pullRequest), "Can't determine pull request state")
    };
}