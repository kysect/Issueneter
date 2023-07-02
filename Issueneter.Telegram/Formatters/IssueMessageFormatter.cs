using Issueneter.Domain.Models;

namespace Issueneter.Telegram.Formatters;

public class IssueMessageFormatter : IMessageFormatter<Issue>
{
    public string ToMessage(Issue entity)
    {
        return $"изи [ишуя]({entity.Url})";
    }
}