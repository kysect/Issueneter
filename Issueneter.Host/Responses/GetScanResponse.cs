namespace Issueneter.Host.Responses;

public record GetScanResponse(int Id, int Type, string AccOrOrg, string Repo, string Filters);