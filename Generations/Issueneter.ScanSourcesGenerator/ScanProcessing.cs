using Issueneter.Annotation;
using Issueneter.ScanSourcesGenerator.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Issueneter.ScanSourcesGenerator;

internal static class ScanProcessing
{
    private static readonly string ScanPropertyTrigger = nameof(ScanPropertyAttribute).ToLowerNoAttributePostfix();
    private static readonly string ScanIgnoreTrigger = nameof(ScanIgnoreAttribute).ToLowerNoAttributePostfix();

    public static ModelProperties ToAvailableProperties(ClassDeclarationSyntax syntax)
    {
        var entityName = syntax.GetClassName();

        var props = syntax
            .GetProperties()
            .Select(k => k.GetScanPropertyName()?.Trim())
            .Where(k => k is not null)
            .ToArray();

        return new ModelProperties(entityName, props);
    }
    
    private static string? GetScanPropertyName(this PropertyDeclarationSyntax prop)
    {
        var attributes = prop.GetAttributes();
        foreach (var attribute in attributes)
        {
            var attrName = attribute.ToLowerNoAttributePostfix();
            if (attrName.Equals(ScanIgnoreTrigger))
                return null;

            if (attrName.Equals(ScanPropertyTrigger))
                return GetPublicAnnotatedPropertyName(attribute, prop.GetName());
        }

        return prop.GetName();
    }
    
    private static string GetPublicAnnotatedPropertyName(AttributeSyntax atr, string defaultName)
    {
        var args = atr.GetArguments().ToList();

        if (args.Count == 0)
            return defaultName;

        var overrideName = args[0];
        return overrideName.Expression.ToFullString().Replace("\"", string.Empty);
    }
}