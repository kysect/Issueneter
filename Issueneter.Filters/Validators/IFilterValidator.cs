using Issueneter.Annotation;

namespace Issueneter.Filters.Validators;

public interface IFilterValidator<in TFilter, TFilterable> where TFilter : IFilter<TFilterable> where TFilterable : IFilterable
{
    public static abstract bool Validate(TFilter filter);
}