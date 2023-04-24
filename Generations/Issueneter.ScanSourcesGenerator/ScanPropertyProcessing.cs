using Issueneter.Annotation;
using Issueneter.ScanSourcesGenerator.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Issueneter.ScanSourcesGenerator;

internal static class ScanPropertyProcessing
{
    private static readonly string ScanPublicTrigger = nameof(ScanPublicAttribute).ToLowerNoAttributePostfix();
    private static readonly string ScanInternalTrigger = nameof(ScanInternalAttribute).ToLowerNoAttributePostfix();

    private static string GetPublicAnnotatedPropertyName(AttributeSyntax atr, string defaultName)
    {
        var args = atr.GetArguments().ToList();

        if (args.Count == 0)
            return defaultName;

        var overrideName = args[0];
        return overrideName.Expression.ToFullString().Replace("\"", string.Empty);
    }
    
    private static string? GetScanPropertyName(this PropertyDeclarationSyntax prop)
    {
        var attributes = prop.GetAttributes();
        foreach (var attribute in attributes)
        {
            var attrName = attribute.ToLowerNoAttributePostfix();
            if (attrName.Equals(ScanInternalTrigger))
                return null;

            if (attrName.Equals(ScanPublicTrigger))
                return GetPublicAnnotatedPropertyName(attribute, attrName);
        }

        return prop.GetName();
    }
    
    public static (string Entity, string[] Properties) ToAvailableProperties(ClassDeclarationSyntax syntax)
    {
        var entityName = syntax.GetClassName();

        var props = syntax
            .GetProperties()
            .Select(k => k.GetScanPropertyName())
            .Where(k => k is not null);

        return (entityName, props.ToArray())!;
    }
}