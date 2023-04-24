using Issueneter.Annotation;
using Issueneter.Domain.Utility;

namespace Issueneter.Domain.Models;

public enum IssueState
{
    Opened = 1,
    Completed = 2,
    NotPlanned = 4,
    Closed = Completed | NotPlanned
}

public class Issue : IFilterable
{
    public Issue(string title, string author, string url, IssueState state, IReadOnlyList<string> labels, Ref<List<TimelineEvent>> events)
    {
        Title = title;
        Author = author;
        Url = url;
        State = state;
        Labels = labels;
        Events = events;
    }
    
    [ScanPublic("Aboba")]
    public string Title { get; init; }
    public string Author { get; init; }
    [ScanInternal]
    public string Url { get; init; }
    public IssueState State { get; init; }
    public IReadOnlyList<string> Labels { get; init; }
    public Ref<List<TimelineEvent>> Events { get; init; }
}