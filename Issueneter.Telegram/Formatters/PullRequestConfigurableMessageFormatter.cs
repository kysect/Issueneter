using Issueneter.Domain.Models;

namespace Issueneter.Telegram.Formatters;

public class PullRequestConfigurableMessageFormatter : IMessageFormatter<PullRequest>
{
    private readonly string _template;

    public PullRequestConfigurableMessageFormatter(string template)
    {
        _template = template;
    }

    public string ToMessage(PullRequest entity)
    {
        return _template
            .Replace("{value.Title}", entity.Title)
            .Replace("{value.Url}", entity.Url);
    }
}