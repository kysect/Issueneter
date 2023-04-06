using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Issueneter.Host.TempDirecory;

public class JsonFilterConverter : CustomCreationConverter<object>
{
    public override object Create(Type objectType)
    {
        throw new NotImplementedException();
    }
    public object Create(Type objectType, JObject jObject)
    {
        var type = (string)jObject.Property("Type");

        switch (type.ToLower())
        {
            case "label":
                return new LabelFilter();
            case "author":
                return new AuthorFilter();
            case "dynamic":
                return new DynamicFilter();
            case "or":
                return new OrFilter();
            case "and":
                return new AndFilter();
        }

        throw new Exception("");
    }
    

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jObject = JObject.Load(reader);
        var target = Create(objectType, jObject);
        serializer.Populate(jObject.CreateReader(), target);
        return target;
    }
}