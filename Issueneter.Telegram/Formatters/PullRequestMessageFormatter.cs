using Issueneter.Domain.Models;

namespace Issueneter.Telegram.Formatters;

public class PullRequestMessageFormatter : IMessageFormatter<PullRequest>
{
    public string ToMessage(PullRequest entity)
    {
        return $"[пр]({entity.Url})";
    }
}