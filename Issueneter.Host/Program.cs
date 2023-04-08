using Hangfire;
using Issueneter.Domain.Models;
using Issueneter.Filters;
using Issueneter.Host.Composition;
using Issueneter.Host.Requests;
using Issueneter.Host.TempDirecory;
using Issueneter.Persistence;
using Issueneter.Runner;
using Issueneter.Telegram;
using Issueneter.Telegram.Formatters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PullRequest = Issueneter.Domain.Models.PullRequest;
using ScanType = Issueneter.Persistence.ScanType;

/*var productInformation = new ProductHeaderValue("ISSUENETER", "1.0.0");
var client = new GitHubClient(productInformation);
client.Credentials = new Credentials("");
var service = new GithubApiService(client);

var issues = await service.GetIssues(DateTimeOffset.Now - TimeSpan.FromHours(3), new ActivitySource("dotnet", "runtime"));

var events = await issues.ElementAt(0).Events.Load();

Console.WriteLine(issues.Count);
*/


var t = "{\"Type\":\"And\",\"Left\":{\"Type\":\"Author\",\"Value\":\"Opened\"},\"Right\":{\"Type\":\"Dynamic\",\"Name\":\"Name\",\"Value\":\"Mr0N\",\"Operand\":\"Equals\"}}";

var json = """
    {
       "Type":"And",
       "Left":{
          "Type" : "Author",
          "Value": "Opened"
       },
       "Right":{
          "Type" : "Dynamic",
             "Name": "Name",
             "Value" : "Mr0N",
             "Operand" : "Equals"
       }
    }
    """;


var parsed = JsonConvert.DeserializeObject<IFilter<PullRequest>>(json, new JsonFilterConverter<PullRequest>());
var newJson = JsonConvert.SerializeObject(parsed, new JsonFilterConverter<PullRequest>());

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services
    .AddSingleton<IMessageFormatter<Issue>, IssueMessageFormatter>()
    .AddSingleton<IMessageFormatter<PullRequest>, PullRequestMessageFormatter>();

services
    .AddDbRelated(builder.Configuration)
    .AddHangfireRelated()
    .AddSwaggerRelated();

var app = builder.Build();

app.UseSwagger().UseSwaggerUI();
app.UseHangfireDashboard();
app.MapHangfireDashboard();


// TODO: Засурсгенить
var availables = new Dictionary<string, Available[]>
{
    ["Issue"] = new[] {new Available("Issue author", "CreatedBy"), new Available("Is open", "State")},
    ["PullRequest"] = new[] {new Available("Pull request author", "CreatedBy"), new Available("State", "State")}
};

app.MapGet("/available_sources", () => availables).WithOpenApi();

app.MapGet("/scans", async (ScanStorage store) => await GetScans(store)).WithOpenApi();
app.MapGet("/scan/{id}", async (int id, ScanStorage store) =>
{
    var scan = await GetScan(store, id);
    if (scan is not null)
        return Results.Ok(scan);

    return Results.NotFound();
}).WithOpenApi();

app.MapPost("/{source}/scan", async (string source, ScanStorage store, [FromBody] AddNewRepoScanRequest request) =>
{
    // TODO: Засурсгенить
    if (source.ToLowerInvariant() == "pullrequest")
    {
        var repoFilters = JsonConvert.DeserializeObject<IFilter<PullRequest>>(request.Filters, new JsonFilterConverter<PullRequest>());
        var creation = new ScanCreation(ScanType.PullRequest, request.Owner, request.Repo, request.Filters);
        var scanId = await store.CreateNewScan(creation);
        
        RecurringJob.AddOrUpdate<ScanRunner>(scanId.ToString(), (runner) => runner.Run(scanId), "3 * * * *");
        return Results.Ok();
    }

    return Results.NotFound();
}).WithOpenApi();
app.Run();

async Task<IReadOnlyCollection<int>> GetScans(ScanStorage store)
{
    return await store.GetAllScansIds();
}

async Task<ScanEntry?> GetScan(ScanStorage store, int id)
{
    return await store.GetScan(id);
}

public class Available
{
    public Available(string name, string field)
    {
        Name = name;
        Field = field;
    }

    public string Name { get; }
    public string Field { get; }
}