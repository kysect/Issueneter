using Issueneter.Annotation;
using Issueneter.Filters.PredefinedFilters;

namespace Issueneter.Filters.Validators;

public class ComplexFilterValidator<TFilterable> : IFilterValidator<ComplexFilter<TFilterable>, TFilterable> where TFilterable : IFilterable
{
    public static bool Validate(ComplexFilter<TFilterable> filter)
    {
        return FilterValidatorApplier.Validate(filter.Left) && FilterValidatorApplier.Validate(filter.Right);
    }
}