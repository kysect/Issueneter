using Issueneter.Domain.Models;
using Octokit;
using Issue = Octokit.Issue;

namespace Issueneter.Github.Utility;

public static class IssueExtension
{
    public static IssueState GetState(this Issue issue)
    {
        return issue switch
        {
            { State.Value: ItemState.Closed, StateReason.Value: ItemStateReason.Completed } => IssueState.Completed,
            { State.Value: ItemState.Closed, StateReason.Value: ItemStateReason.NotPlanned } => IssueState.NotPlanned,
            { State.Value: ItemState.Open } => IssueState.Opened,
            _ => throw new ArgumentOutOfRangeException(nameof(issue), "Can't determine issue state")
        };
    }
}