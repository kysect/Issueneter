using Issueneter.Domain.Models;
using Octokit;

namespace Issueneter.Github.Utility;

public static class EventInfoExtensions
{
    public static TimelineEvent ToTimelineEvent(this TimelineEventInfo eventInfo) => eventInfo.Event.Value switch
    {
        EventInfoState.Labeled => new LabeledEvent(eventInfo.CreatedAt, eventInfo.Label.Name),
        _ => new UndefinedEvent(eventInfo.CreatedAt)
    };
}