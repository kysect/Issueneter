using Issueneter.Domain;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Issueneter.Telegram;

public class TelegramSender<T> where T : IFilterable
{
    private readonly TelegramBotClient _botClient;
    private readonly IMessageFormatter<T> _messageFormatter;

    public TelegramSender(TelegramBotClient botClient, IMessageFormatter<T> messageFormatter)
    {
        _botClient = botClient;
        _messageFormatter = messageFormatter;
    }

    public async Task SendResults(ChatId chat, IReadOnlyCollection<T> results)
    {
        foreach (var result in results)
        {
            var message = _messageFormatter.ToMessage(result);
            await _botClient.SendTextMessageAsync(chat, message, ParseMode.Markdown);
        }
    }
}