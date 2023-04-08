namespace Issueneter.Persistence;

// TODO: Засурсгенить
public enum ScanType
{
    Issue = 1,
    PullRequest = 2
}

public record ScanCreation(ScanType Type, string Owner, string Repo, long ChatId, string Filters);