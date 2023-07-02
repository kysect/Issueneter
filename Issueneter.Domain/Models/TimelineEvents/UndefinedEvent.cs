namespace Issueneter.Domain.Models;

public class UndefinedEvent : TimelineEvent
{
    public UndefinedEvent(DateTimeOffset timestamp) : base(timestamp)
    {
    }
}