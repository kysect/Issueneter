namespace Issueneter.Domain.Models;

public abstract class TimelineEvent
{
    protected TimelineEvent(DateTimeOffset timestamp)
    {
        Timestamp = timestamp;
    }

    public DateTimeOffset Timestamp { get; init; }
}