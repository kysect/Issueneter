using Issueneter.Host.Composition;
using Issueneter.Telegram;
using Telegram.Bot;

namespace Issueneter.Host.Modules;

public static class TelegramModule
{
    public static IServiceCollection AddTelegramModule(this IServiceCollection services, IHostEnvironment env, IConfigurationRoot configuration) =>
        services
            .Configure<TelegramOptions>(configuration.GetSection(nameof(TelegramOptions)))
            .AddSingleton<TelegramSender>()
            .AddSingleton<TelegramBotClient>(sp => new TelegramBotClient(sp.GetRequiredOptions<TelegramOptions>().Token));
}