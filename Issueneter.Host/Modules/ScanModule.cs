using Issueneter.Domain.Models;
using Issueneter.Host.Composition;
using Issueneter.Runner;
using Issueneter.Telegram;
using Issueneter.Telegram.Formatters;

namespace Issueneter.Host.Modules;

public static class ScanModule
{
    public static IServiceCollection AddScanModule(this IServiceCollection services, IHostEnvironment env, IConfigurationRoot configuration) =>
        services
            .AddSingleton(GetIssueFormatter)
            .AddSingleton(GetPullRequestFormatter)
            .AddSingleton<ScanRunner>();

    private static IMessageFormatter<Issue> GetIssueFormatter(IServiceProvider serviceProvider)
    {
        var options = serviceProvider.GetRequiredOptions<TelegramOptions>();

        if (options.IssueMessageTemplate is not null)
            return new IssueConfigurableMessageFormatter(options.IssueMessageTemplate);

        return new IssueMessageFormatter();
    }

    private static IMessageFormatter<PullRequest> GetPullRequestFormatter(IServiceProvider serviceProvider)
    {
        var options = serviceProvider.GetRequiredOptions<TelegramOptions>();

        if (options.PullRequestMessageTemplate is not null)
            return new PullRequestConfigurableMessageFormatter(options.PullRequestMessageTemplate);

        return new PullRequestMessageFormatter();
    }
}