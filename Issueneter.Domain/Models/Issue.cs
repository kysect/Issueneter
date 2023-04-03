using Issueneter.Domain.Utility;

namespace Issueneter.Domain.Models;

public class Issue
{
    public Issue(string title, string author, string url, List<string> labels, Ref<List<TimelineEvent>> events)
    {
        Title = title;
        Author = author;
        Url = url;
        Labels = labels;
        Events = events;
    }

    public string Title { get; init; }
    public string Author { get; init; }
    public string Url { get; init; }
    public IReadOnlyList<string> Labels { get; init; }
    public Ref<List<TimelineEvent>> Events { get; init; }
}