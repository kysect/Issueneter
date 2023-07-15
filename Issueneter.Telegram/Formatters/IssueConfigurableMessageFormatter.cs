using Issueneter.Domain.Models;

namespace Issueneter.Telegram.Formatters;

public class IssueConfigurableMessageFormatter : IMessageFormatter<Issue>
{
    private readonly string _template;

    public IssueConfigurableMessageFormatter(string template)
    {
        _template = template;
    }

    public string ToMessage(Issue entity)
    {
        return _template
            .Replace("{value.Title}", entity.Title)
            .Replace("{value.Url}", entity.Url);
    }
}