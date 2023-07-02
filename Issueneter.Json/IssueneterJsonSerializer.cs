using Issueneter.Annotation;
using Issueneter.Filters;
using Issueneter.Filters.Validators;
using Newtonsoft.Json;

namespace Issueneter.Json;

public class IssueneterJsonSerializer
{
    public static IFilter<TFilterable> Deserialize<TFilterable>(string data) where TFilterable : IFilterable
    {
        var filter = JsonConvert.DeserializeObject<IFilter<TFilterable>>(data, new JsonFilterConverter<TFilterable>());
        if (!FilterValidatorApplier.Validate(filter))
            // TODO: Change exception
            throw new InvalidDataException();

        return filter;
    }

    public static string Serialize<TFilterable>(IFilter<TFilterable> filter) where TFilterable : IFilterable
    {
        return JsonConvert.SerializeObject(filter, new JsonFilterConverter<TFilterable>());
    }
}