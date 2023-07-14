using Issueneter.Annotation;
using Issueneter.Domain;

namespace Issueneter.Filters;

public class DynamicFilter<T> : IFilter<T> where T : IFilterable
{
    public string Name { get; init; }
    public string Value { get; init; }
    
    public bool Apply(T entity)
    {
        return entity.GetProperty(Name) == Value;
    }
}