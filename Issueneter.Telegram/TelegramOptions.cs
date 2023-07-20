namespace Issueneter.Telegram;

public class TelegramOptions
{
    public string? Token { get; init; }
    public string? IssueMessageTemplate { get; init; }
    public string? PullRequestMessageTemplate { get; init; }
}