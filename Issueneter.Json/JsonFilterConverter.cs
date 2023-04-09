using System.Collections;
using System.Reflection;
using Issueneter.Domain;
using Issueneter.Filters;
using Issueneter.Filters.PredefinedFilters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Issueneter.Json;

public class JsonFilterConverter<T> : JsonConverter<IFilter<T>> where T : IFilterable
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

    public override void WriteJson(JsonWriter writer, IFilter<T> value, JsonSerializer serializer)
    {
        var jObject = new JObject();
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

        Type valueType = value.GetType();
        if (valueType.IsArray)
        {
            var jArray = new JArray();
            foreach (var item in (IEnumerable)value)
                jArray.Add(JToken.FromObject(item, serializer));

            jArray.WriteTo(writer);
        }
        else
        {
            foreach (var property in valueType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanRead) 
                    continue;
                
                var propertyValue = property.GetValue(value);
                if (propertyValue != null)
                    jObject.Add(property.Name, JToken.FromObject(propertyValue, serializer));
            }

            jObject.WriteTo(writer);
        }
    }

    public override IFilter<T> ReadJson(JsonReader reader, Type objectType, IFilter<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jObject = JObject.Load(reader);
        var target = Create(jObject);
        serializer.Populate(jObject.CreateReader(), target);
        return (IFilter<T>)target;
    }
}