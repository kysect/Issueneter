using Issueneter.Annotation;
using Issueneter.Filters.PredefinedFilters;

namespace Issueneter.Filters.Validators;

// TODO: SourceGen
public static class FilterValidatorApplier
{
    public static bool Validate<TFilterable>(IFilter<TFilterable> filter) where TFilterable : IFilterable
    {
        return filter switch
        {
            LabelFilter f => Validate<TFilterable>(f),
            AuthorFilter f => Validate<TFilterable>(f),
            StateFilter f => Validate<TFilterable>(f),
            ComplexFilter<TFilterable> f => Validate(f),
            DynamicFilter<TFilterable> f => Validate(f),
            _ => throw new ArgumentOutOfRangeException(nameof(filter)),
        };
    }
    
    private static bool Validate<TFilterable>(LabelFilter filter) where TFilterable : IFilterable
    {
        return true;
    }
    
    private static bool Validate<TFilterable>(AuthorFilter filter) where TFilterable : IFilterable
    {
        return true;
    }
    
    private static bool Validate<TFilterable>(StateFilter filter) where TFilterable : IFilterable
    {
        return true;
    }
    
    private static bool Validate<TFilterable>(ComplexFilter<TFilterable> filter) where TFilterable : IFilterable
    {
        return ComplexFilterValidator<TFilterable>.Validate(filter);
    }
    
    private static bool Validate<TFilterable>(DynamicFilter<TFilterable> filter) where TFilterable : IFilterable
    {
         return DynamicFilterValidator<TFilterable>.Validate(filter);
    }
}