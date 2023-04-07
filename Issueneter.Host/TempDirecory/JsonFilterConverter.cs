using Issueneter.Domain;
using Issueneter.Filters;
using Issueneter.Filters.PredefinedFilters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Octokit;
using PullRequest = Issueneter.Domain.Models.PullRequest;

namespace Issueneter.Host.TempDirecory;

public class JsonFilterConverter<T> : CustomCreationConverter<IFilter<T>> where T : IFilterable
{
    public override IFilter<T> Create(Type objectType)
    {
        throw new NotImplementedException();
    }
    public object Create(JObject jObject)
    {
        var type = (string)jObject.Property("Type");

        switch (type.ToLower())
        {
            case "or":
                return new ComplexFilter<T>(ComplexOperand.Or);
            case "and":
                return new ComplexFilter<T>(ComplexOperand.And);
            default:
                return new AuthorFilter();
        }

        throw new Exception("");
    }
    

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jObject = JObject.Load(reader);
        var target = Create(jObject);
        serializer.Populate(jObject.CreateReader(), target);
        return target;
    }
}