namespace Issueneter.Host.Requests;

public record AddNewRepoScanRequest(string Owner, string Repo, long ChatId, string Filters);