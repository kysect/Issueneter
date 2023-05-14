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
            .Select(k => k.GetScanProperty())
            .Where(k => k is not null)
            .ToArray();

        return new ModelProperties(entityName, props);
    }
    
    private static ModelProperty GetScanProperty(this PropertyDeclarationSyntax prop)
    {
        var attributes = prop.GetAttributes();
        var propertyName = prop.GetName().Trim();
        foreach (var attribute in attributes)
        {
            var attrName = attribute.ToLowerNoAttributePostfix();
            if (attrName.Equals(ScanIgnoreTrigger))
                return null;

            if (attrName.Equals(ScanPropertyTrigger))
                return GetPublicAnnotatedProperty(attribute, propertyName);
        }

        return new ModelProperty(propertyName,  propertyName);
    }
    
    private static ModelProperty GetPublicAnnotatedProperty(AttributeSyntax atr, string propertyName)
    {
        var args = atr.GetArguments().ToList();

        if (args.Count == 0)
            return new ModelProperty(propertyName, propertyName);

        var overrideName = args[0];
        return new ModelProperty(overrideName.Expression.ToFullString().Replace("\"", string.Empty), propertyName);
    }
}