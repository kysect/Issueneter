using Issueneter.Annotation;

namespace Issueneter.Filters.Validators;

public interface IFilterValidator<TFilter, TFilterable> where TFilter : IFilter<TFilterable> where TFilterable : IFilterable
{
    public bool Validate(TFilter filter);
}