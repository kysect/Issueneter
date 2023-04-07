using Issueneter.Domain;

namespace Issueneter.Filters;

public class DynamicFilter<T> : IFilter<T> where T : IFilterable
{
    public bool Apply(T entity)
    {
        return true;
    }
}