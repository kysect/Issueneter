using Hangfire;
using Issueneter.Domain.Models;
using Issueneter.Filters;
using Issueneter.Host.Requests;
using Issueneter.Json;
using Issueneter.Persistence;
using Issueneter.Runner;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Issueneter.Host.Routes;

public static class ScanRoutes
{
    public static WebApplication MapScanRoutes(this WebApplication app)
    {
        app.MapGet("/scans", async (ScanStorage store) => await GetScans(store)).WithOpenApi();
        app.MapGet("/scan/{id}", async (long id, ScanStorage store) => await GetScan(store, id)).WithOpenApi();
        app.MapPost("/{source}/scan", async (string source, ScanStorage store, [FromBody] AddNewRepoScanRequest request) 
            => await CreateScan(store, source, request)).WithOpenApi();
        app.MapPost("/scan/{id}/force", async (ScanRunner runner, long scanId) => await ForceScan(runner, scanId)).WithOpenApi();

        return app;
    }

    private static async Task<IResult> GetScans(ScanStorage store)
    {
        return Results.Ok(await store.GetAllScansIds());
    }

    private static async Task<IResult> GetScan(ScanStorage storage, long scanId)
    {
        var scan = await storage.GetScan(scanId);
        
        if (scan is not null)
            return Results.Ok(scan);

        return Results.NotFound();
    }

    private static async Task<IResult> ForceScan(ScanRunner runner, long scanId)
    {
        await runner.Run(scanId);
        return Results.Ok();
    }
    
    private static async Task<IResult> CreateScan(ScanStorage store, string source, AddNewRepoScanRequest request)
    {
        // TODO: Засурсгенить
        if (source.ToLowerInvariant() == "issue")
        {
            var repoFilters = JsonConvert.DeserializeObject<IFilter<Issue>>(request.Filters, new JsonFilterConverter<Issue>());
            var creation = new ScanCreation(ScanType.Issue, request.Owner, request.Repo, request.ChatId, request.Filters);
            var scanId = await store.CreateNewScan(creation);
        
            RecurringJob.AddOrUpdate<ScanRunner>(scanId.ToString(), (runner) => runner.Run(scanId), "* * * * *");
            return Results.Ok();
        }

        return Results.NotFound();
    }
}