using Issueneter.Domain;
using Issueneter.Filters;
using Issueneter.Filters.PredefinedFilters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Octokit;
using PullRequest = Issueneter.Domain.Models.PullRequest;

namespace Issueneter.Host.TempDirecory;

public class JsonFilterConverter<T> : JsonConverter where T : IFilterable
{
    public object Create(JObject jObject)
    {
        var type = (string)jObject.Property("Type");

        switch (type.ToLower())
        {
            case "or":
                return new ComplexFilter<T>(ComplexOperand.Or);
            case "and":
                return new ComplexFilter<T>(ComplexOperand.And);
            case "label":
                return new LabelFilter();
            case "author":
                return new AuthorFilter();
            case "state":
                return new StateFilter();
            case "dynamic":
                return new DynamicFilter<T>();
        }

        throw new Exception("");
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var jToken = JToken.FromObject(value, serializer);

        if (jToken.Type != JTokenType.Object || value is not IFilter<T>)
        {
            jToken.WriteTo(writer);
        }

        var jObject = (JObject)jToken;
        var type = value switch
        {
            ComplexFilter<T> complexFilter => complexFilter.Operand == ComplexOperand.And ? "and" : "or",
            LabelFilter => "label",
            AuthorFilter => "author",
            StateFilter => "state",
            DynamicFilter<T> => "dynamic",
            _ => throw new ArgumentOutOfRangeException(nameof(value),"")
        };
        jObject.AddFirst(new JProperty("Type", type));
        jObject.WriteTo(writer);
    }

    public override IFilter<T> ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jObject = JObject.Load(reader);
        var target = Create(jObject);
        serializer.Populate(jObject.CreateReader(), target);
        return (IFilter<T>)target;
    }

    public override bool CanConvert(Type objectType)
    {
        return typeof(IFilter<T>).IsAssignableFrom(objectType);
    }
}