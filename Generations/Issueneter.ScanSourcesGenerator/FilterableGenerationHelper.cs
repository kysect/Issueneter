using System.Text;

namespace Issueneter.ScanSourcesGenerator;

public static class FilterableGenerationHelper
{
    private const string Start = """
        using System;
        using System.Collections.Generic;
        using Issueneter.Annotation;
        using Issueneter.Mappings;
        
        namespace Issueneter.Domain.Models;
        
        public partial class {0} : IFilterable
        {{
            public string GetProperty(string name) => name.ToLower() switch
            {{

        """;

    private const string End = """
                _ => throw new ArgumentOutOfRangeException(nameof(name), $"Not expected property name: {{name}}"),
            }};

            public static ScanType ScanType => ScanType.{0};
        }}
        """;

    private static string WrapWithQuotes(string str) => $"\"{str}\"";
    
    public static string Generate(ModelProperties model)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat(Start, model.Name);

        foreach (var property in model.Properties)
        {
            stringBuilder.AppendLine($"\t\t{WrapWithQuotes(property.Name.ToLower())} => {property.FieldName}.ToString(),");
        }

        stringBuilder.AppendFormat(End, model.Name);
        return stringBuilder.ToString();
    }
}