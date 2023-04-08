using Hangfire;
using Issueneter.Filters;
using Issueneter.Github;
using Issueneter.Host.Composition;
using Issueneter.Host.Options;
using Issueneter.Host.Requests;
using Issueneter.Host.TempDirecory;
using Issueneter.Persistence;
using Issueneter.Runner;
using Issueneter.Telegram;
using Issueneter.Telegram.Formatters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Octokit;
using Telegram.Bot;
using Issue = Issueneter.Domain.Models.Issue;
using PullRequest = Issueneter.Domain.Models.PullRequest;
using ScanType = Issueneter.Persistence.ScanType;


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


var q = """ 
{
"Left": {
    "Type": "author",
    "Value": "BruceForstall"
},
"Right": {
    "Type": "state",
    "Value": 1
},
"Operand": 2
}
""";

var parsed = JsonConvert.DeserializeObject<IFilter<PullRequest>>(json, new JsonFilterConverter<PullRequest>());
var newJson = JsonConvert.SerializeObject(parsed, new JsonFilterConverter<PullRequest>());

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json");
var services = builder.Services;

services.Configure<GithubOptions>(builder.Configuration.GetSection(nameof(GithubOptions)));
services.Configure<TelegramOptions>(builder.Configuration.GetSection(nameof(TelegramOptions)));
services
    .AddSingleton<ScanRunner>()
    .AddTransient<TelegramSender>()
    .AddSingleton<TelegramBotClient>(sp =>
    {
        var token = sp.GetRequiredOptions<TelegramOptions>().Token;
        return new TelegramBotClient(token);
    });
services.AddSingleton<GithubApiService>()
    .AddSingleton<IGitHubClient>(sp =>
    {
        var productInformation = new ProductHeaderValue("ISSUENETER", "1.0.0");
        var client = new GitHubClient(productInformation);
        client.Credentials = new Credentials(sp.GetRequiredOptions<GithubOptions>().Token);
        return client;
    });
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

app.MapPost("/scan/{id}/force", async (ScanRunner runner, long scanId) =>
{
    await runner.Run(scanId);
    return Results.Ok();
}).WithOpenApi();

app.MapPost("/{source}/scan", async (string source, ScanStorage store, [FromBody] AddNewRepoScanRequest request) =>
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