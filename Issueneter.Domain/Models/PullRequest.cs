using Issueneter.Annotation;
using Issueneter.Domain.Utility;

namespace Issueneter.Domain.Models;

public enum PullRequestState
{
    Draft = 1,
    Active = 2,
    Opened = Draft | Active,
    Merged = 4,
    Aborted = 8,
    Closed = Merged | Aborted
}

public class PullRequest : IFilterable
{
    public PullRequest(string title, string author, string url, PullRequestState state, IReadOnlyList<string> labels, Ref<List<TimelineEvent>> events)
    {
        Title = title;
        Author = author;
        Url = url;
        State = state;
        Labels = labels;
        Events = events;
    }

    public string Title { get; init; }
    public string Author { get; init; }
    public string Url { get; init; }
    public PullRequestState State { get; init; }
    public IReadOnlyList<string> Labels { get; init; }
    public Ref<List<TimelineEvent>> Events { get; init; }
}