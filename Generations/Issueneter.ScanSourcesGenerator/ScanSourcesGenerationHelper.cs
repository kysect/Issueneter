using System.Text;
using Issueneter.Annotation;

namespace Issueneter.ScanSourcesGenerator;

public class ScanSourcesGenerationHelper
{
    private const string Start = @"
using System;
using System.Collections.Generic;
using Issueneter.Annotation;

namespace Mappings;

public static class ModelsInfo
{
    private static readonly List<AvailableSource> _availableScanSources = new List<AvailableScanSource>()
    {
";

    private const string End = @"
    };

    public static ICollection<AvailableScanSource> AvailableScanSources => _availableSources;
}";
    

    private static string WrapWithQuotes(string str) => $"\"{str}\"";
    

    public static string Generate(IEnumerable<ModelProperties> sources)
    {
        var builder = new StringBuilder();
        builder.Append(Start);
        foreach (var source in sources)
        {
            builder.Append($"\t\tnew AvailableScanSource({WrapWithQuotes(source.Name)}, new List<string>(){{{string.Join(", ", source.Properties.Select(WrapWithQuotes))}}}),\n");
        }

        builder.Append(End);
        return builder.ToString();
    }
}