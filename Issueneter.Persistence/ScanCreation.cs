using Issueneter.Mappings;

namespace Issueneter.Persistence;

public record ScanCreation(ScanType Type, string Owner, string Repo, long ChatId, string Filters);