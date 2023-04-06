namespace Issueneter.Host.Requests;

public record AddNewRepoScanRequest(string Owner, string Repo, string Filters);