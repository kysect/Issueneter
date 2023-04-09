using Issueneter.Host.Modules;
using Issueneter.Host.Routes;
using Issueneter.Runner;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var env = builder.Environment;
var config = builder.Configuration;

if (env.IsDevelopment())
    config.AddJsonFile("appsettings.Local.json");

services
    .AddDatabaseModule(env, config)
    .AddGithubModule(env, config)
    .AddHangfireModule(env, config)
    .AddSwaggerModule(env, config)
    .AddTelegramModule(env, config)
    .AddSingleton<ScanRunner>();

var app = builder.Build();

app
    .UseHangfireModule()
    .UseSwaggerModule()
    .MapScanRoutes()
    .MapScanSourceRoutes()
    .Run();