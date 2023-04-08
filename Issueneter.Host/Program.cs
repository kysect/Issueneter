using Issueneter.Host.Modules;
using Issueneter.Host.Routes;
using Issueneter.Runner;
using Issueneter.Telegram;
using Issueneter.Telegram.Formatters;
using PullRequest = Issueneter.Domain.Models.PullRequest;
using Issue = Issueneter.Domain.Models.Issue;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json");

var services = builder.Services;
var env = builder.Environment;
var config = builder.Configuration;

services
    .AddSingleton<ScanRunner>()
    .AddSingleton<IMessageFormatter<Issue>, IssueMessageFormatter>()
    .AddSingleton<IMessageFormatter<PullRequest>, PullRequestMessageFormatter>();

services
    .AddDatabaseModule(env, config)
    .AddGithubModule(env, config)
    .AddHangfireModule(env, config)
    .AddSwaggerModule(env, config)
    .AddTelegramModule(env, config);

var app = builder.Build();

app
    .UseHangfireModule()
    .UseSwaggerModule()
    .MapScanRoutes()
    .MapScanSourceRoutes()
    .Run();