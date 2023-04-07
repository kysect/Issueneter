using Issueneter.Domain;

namespace Issueneter.Filters;

public class DynamicFilter<T> : IFilter<T> where T : IFilterable
{
    public string Name { get; set; }
    public string Value { get; set; }
    public bool Apply(T entity)
    {
        return true;
    }
}