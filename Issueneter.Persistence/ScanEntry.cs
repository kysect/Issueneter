namespace Issueneter.Persistence;

public record ScanEntry(long Id, short ScanType, string Owner, string Repo, DateTime Created, string Filters);