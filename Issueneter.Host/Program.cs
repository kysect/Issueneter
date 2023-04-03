using Hangfire;
using Hangfire.PostgreSql;
using Issueneter.ApiModels.Responses;
using Issueneter.Domain.Models;
using Issueneter.Github;
using Octokit;

var productInformation = new ProductHeaderValue("ISSUENETER", "1.0.0");
var client = new GitHubClient(productInformation);
client.Credentials = new Credentials("");
var service = new GithubApiService(client);

var issues = await service.GetIssues(DateTimeOffset.Now - TimeSpan.FromHours(3), new ActivitySource("dotnet", "runtime"));

var events = await issues.ElementAt(0).Events.Load();

Console.WriteLine(issues.Count());

return;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddHangfire(config =>
{
    config
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UsePostgreSqlStorage("User ID=postgres;Password=postgres;Host=192.168.1.116;Port=5432;Database=postgres;Pooling=true;Integrated Security=true;");
});

services.AddHangfireServer();
var app = builder.Build();

app.UseHangfireDashboard();
app.MapHangfireDashboard();

app.MapGet("/", () =>
{
    Handle();
    return "Ok";
});

app.MapGet("/scans", async () => await GetScans());
app.MapGet("/scan/{id}", async (int id) => await GetScan(id));

app.MapPost("/scan", async () => await AddNewScanTask());
app.Run();

Task<IReadOnlyCollection<ScansResponse>> GetScans()
{
    return Task.FromResult((IReadOnlyCollection<ScansResponse>)Array.Empty<ScansResponse>());
}

Task<ScanResponse> GetScan(int id)
{
    return null;
}

Task AddNewScanTask()
{
    return Task.CompletedTask;
}

void Handle()
{
    RecurringJob.AddOrUpdate("parse repo", () => Console.WriteLine("aboba"), () => "15 * * * *");
}