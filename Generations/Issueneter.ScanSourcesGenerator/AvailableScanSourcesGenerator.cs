using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Issueneter.ScanSourcesGenerator;

[Generator]
public class AvailableScanSourcesGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new ScanEntitySyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if(context.SyntaxReceiver is not ScanEntitySyntaxReceiver receiver)
            return;

        var start = @"
using System;
using System.Collections.Generic;

namespace Mappings;

public static class FromEntity
{
    public static Dictionary<string, string[]> Values { get; } = new()
    {";
        var builder = new StringBuilder(start);
        builder.AppendLine();
        foreach (var entity in receiver.Entities)
        {
            var results = ScanPropertyProcessing.ToAvailableProperties(entity.Entity);

            builder.AppendLine($"       [\"{results.Entity}\"] = new [] {{{string.Join(", ", results.Properties.Select(k => $"\"{k}\""))}}},");
        }

        if (receiver.Entities.Any())
        {
            builder.Remove(builder.Length - 1, 1);
            builder.AppendLine();
        }

        builder.AppendLine("    };");
        builder.Append("}");
        
        context.AddSource("Generated.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
    }
}