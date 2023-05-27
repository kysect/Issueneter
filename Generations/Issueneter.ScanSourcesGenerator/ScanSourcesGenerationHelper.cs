using System.Text;
using Issueneter.Annotation;

namespace Issueneter.ScanSourcesGenerator;

public static class ScanSourcesGenerationHelper
{
    private const string Start = """
        using System;
        using System.Collections.Generic;
        using Issueneter.Annotation;
        
        namespace Issueneter.Mappings;
        
        public static class ModelsInfo
        {
            private static readonly Dictionary<ScanType, IReadOnlyCollection<string>> _availableSources = new ()
            {

        """;

    private const string End = """
            };
        
            public static IReadOnlyDictionary<ScanType, IReadOnlyCollection<string>> AvailableScanSources => _availableSources;
        }
        """;

    private static string WrapWithQuotes(string str) => $"\"{str}\"";
    
    public static string Generate(IEnumerable<ModelProperties> sources)
    {
        var builder = new StringBuilder();
        builder.Append(Start);
        foreach (var source in sources)
        {
            builder.Append($"\t\t[ScanType.{source.Name}] = new []{{{string.Join(", ", source.Properties.Select(p => WrapWithQuotes(p.Name)))}}},\n");
        }

        builder.Append(End);
        return builder.ToString();
    }
}