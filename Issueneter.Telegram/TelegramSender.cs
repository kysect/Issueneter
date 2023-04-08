using Issueneter.Domain;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Issueneter.Telegram;

public class TelegramSender
{
    private readonly TelegramBotClient _botClient;
    private readonly IMessageFormatter<IFilterable> _messageFormatter;

    public TelegramSender(TelegramBotClient botClient, IMessageFormatter<IFilterable> messageFormatter)
    {
        _botClient = botClient;
        _messageFormatter = messageFormatter;
    }

    public async Task SendResults<T>(ChatId chat, IReadOnlyCollection<T> results) where T : IFilterable
    {
        foreach (var result in results)
        {
            var message = _messageFormatter.ToMessage(result);
            await _botClient.SendTextMessageAsync(chat, message, ParseMode.Markdown);
        }
    }
}