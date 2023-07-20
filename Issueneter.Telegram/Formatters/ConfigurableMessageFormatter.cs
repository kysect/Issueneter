using Issueneter.Annotation;
using Issueneter.Mappings;

namespace Issueneter.Telegram.Formatters;

public class ConfigurableMessageFormatter<T> : IMessageFormatter<T> where T : IFilterable
{
    private readonly string _template;

    public ConfigurableMessageFormatter(string template)
    {
        _template = template;
    }

    public string ToMessage(T entity)
    {
        var result = _template;

        foreach (string property in ModelsInfo.AvailableScanSources[T.ScanType])
            result = result.Replace($"{{value.{property}}}", entity.GetProperty(property));

        return result;
    }
}