using System.Text;

namespace Issueneter.ScanSourcesGenerator;

public class ScanSourcesCodeGenerator
{
    private const string Start = @"
using System;
using System.Collections.Generic;

namespace Mappings;

public static class FromEntity
{
    public static Dictionary<string, string[]> Values { get; } = new()
    {
";

    private const string End = @"
    };
}";

    private readonly StringBuilder _builder = new StringBuilder(Start);

    private string WrapWithQuotes(string str) => $"\"{str}\"";
    
    public void AddEntity(string entity, string[] properties)
    {
        if (_builder.Length != Start.Length)
        {
            _builder.AppendLine(",");
        }
        
        _builder.Append($"       [\"{entity}\"] = new [] {{{string.Join(", ", properties.Select(WrapWithQuotes))}}}");
    }

    public string GetResult() => _builder.Append(End).ToString();
    
}


public static class Ext
{
    public static string Truncate(this string str, int maxLength)
    {
        if (str.Length < maxLength)
            return str;

        return str.Substring(0, maxLength);
    }
    
    public static ReadOnlySpan<char> Truncate(this ReadOnlySpan<char> str, int maxLength)
    {
        if (str.Length < maxLength)
            return str;

        return str.Slice(0, maxLength);
    }
    
    public static string Truncate(this string str, char until)
    {
        var index = str.IndexOf(until);

        if (index == -1)
            return str;

        return str.Substring(0, index);
    }
}