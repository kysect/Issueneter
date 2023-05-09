using Issueneter.Annotation;
using Issueneter.Domain.Utility;
using Mappings;

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

    [ScanProperty]
    public string Title { get; init; }
    [ScanProperty]
    public string Author { get; init; }
    [ScanIgnore]
    public string Url { get; init; }
    public IssueState State { get; init; }
    public IReadOnlyList<string> Labels { get; init; }
    [ScanIgnore]
    public Ref<List<TimelineEvent>> Events { get; init; }
}