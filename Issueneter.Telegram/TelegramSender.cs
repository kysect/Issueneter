using Issueneter.Domain;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Issueneter.Telegram;

public class TelegramSender
{
    private readonly TelegramBotClient _botClient;

    public TelegramSender(TelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task SendResults<T>(ChatId chat, IMessageFormatter<T> formatter, IReadOnlyCollection<T> results) where T : IFilterable
    {
        foreach (var result in results)
        {
            var message = formatter.ToMessage(result);
            await _botClient.SendTextMessageAsync(chat, message, ParseMode.Markdown);
        }
    }
}