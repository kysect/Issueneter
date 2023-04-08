using Issueneter.Domain;

namespace Issueneter.Telegram;

public interface IMessageFormatter<T> where T : IFilterable
{
    string ToMessage(T entity);
}