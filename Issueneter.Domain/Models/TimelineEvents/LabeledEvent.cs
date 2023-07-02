namespace Issueneter.Domain.Models;

public class LabeledEvent : TimelineEvent
{
    public LabeledEvent(DateTimeOffset timestamp, string label) : base(timestamp)
    {
        Label = label;
    }
    
    public string Label { get; init; }
}