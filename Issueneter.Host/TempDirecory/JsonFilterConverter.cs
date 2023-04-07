using Issueneter.Domain;
using Issueneter.Filters;
using Issueneter.Filters.PredefinedFilters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Octokit;
using PullRequest = Issueneter.Domain.Models.PullRequest;

namespace Issueneter.Host.TempDirecory;

public class JsonFilterConverter<T> : CustomCreationConverter<object> where T : IFilterable
{
    public override object Create(Type objectType)
    {
        throw new NotImplementedException();
    }
    public object Create(JObject jObject)
    {
        var type = (string)jObject.Property("Type");

        switch (type.ToLower())
        {
            case "or":
            case "and":
                return new ComplexFilter<T>();
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