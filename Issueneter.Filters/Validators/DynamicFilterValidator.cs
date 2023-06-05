using Issueneter.Annotation;
using Issueneter.Mappings;

namespace Issueneter.Filters.Validators;

public class DynamicFilterValidator<TFilterable> : IFilterValidator<DynamicFilter<TFilterable>, TFilterable> where TFilterable : IFilterable
{
    public bool Validate(DynamicFilter<TFilterable> filter)
    {
        var fields = ModelsInfo.AvailableScanSources[TFilterable.ScanType];
        return fields.Contains(filter.Name);
    }
}