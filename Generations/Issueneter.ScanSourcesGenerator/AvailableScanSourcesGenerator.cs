using System.Diagnostics;
using System.Text;
using Issueneter.Annotation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Issueneter.ScanSourcesGenerator;

[Generator]
public class AvailableScanSourcesGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        if (!Debugger.IsAttached)
            Debugger.Launch();
        
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
        foreach (var entity in receiver.Entities)
        {
            var name = entity.Entity.Identifier.Text;
            var defaults = entity.Defaults.Select(k => k.Identifier.Text);
            builder.AppendLine($"       [{name}] = new [] {{{string.Join(",", defaults)}}},");
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