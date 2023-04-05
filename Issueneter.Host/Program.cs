using System.Text.Json;
using Hangfire;
using Hangfire.PostgreSql;
using Issueneter.ApiModels.Requests;
using Issueneter.ApiModels.Responses;
using Issueneter.Domain.Models;
using Issueneter.Github;
using Issueneter.Host.Composition;
using Issueneter.Host.Options;
using Issueneter.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Octokit;

/*var productInformation = new ProductHeaderValue("ISSUENETER", "1.0.0");
var client = new GitHubClient(productInformation);
client.Credentials = new Credentials("");
var service = new GithubApiService(client);

var issues = await service.GetIssues(DateTimeOffset.Now - TimeSpan.FromHours(3), new ActivitySource("dotnet", "runtime"));

var events = await issues.ElementAt(0).Events.Load();

Console.WriteLine(issues.Count);
*/

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services
    .AddDbRelated(builder.Configuration)
    .AddHangfireRelated()
    .AddSwaggerRelated();

var app = builder.Build();

app.UseSwagger().UseSwaggerUI();
app.UseHangfireDashboard();
app.MapHangfireDashboard();


// TODO: Засурсгенить
var availables = new Dictionary<string, (string name, string property)[]>
{
    ["Issue"] = new[] {("Issue author", "CreatedBy"), ("Is open", "State")},
    ["PullRequest"] = new[] {("Pull request author", "CreatedBy"), ("State", "State")}
};

app.MapGet("/available_sources", () => availables).WithOpenApi();

app.MapGet("/scans", async (ScanStore store) => await GetScans(store)).WithOpenApi();
app.MapGet("/scan/{id}", async (int id, ScanStore store) => await GetScan(store, id)).WithOpenApi();

app.MapPost("/{source}/scan", async (string source, ScanStore store, [FromBody] string json) =>
{
    // TODO: Засурсгенить
    if (source.ToLowerInvariant() == "PullRequest")
    {
        var repoRequest = JsonSerializer.Deserialize<AddNewRepoScanRequest>(json);
        await store.CreateNewScan(repoRequest);
    }

    return Results.NotFound();
}).WithOpenApi();
app.Run();

async Task<IReadOnlyCollection<int>> GetScans(ScanStore store)
{
    return await store.GetAllScansIds();
}

async Task<ScanResponse> GetScan(ScanStore store, int id)
{
    return await store.GetScan(id);
}