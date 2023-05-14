using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Issueneter.ScanSourcesGenerator.Extensions;

public static class AttributeExtensions
{
    public static string ToLowerNoAttributePostfix(this string str) =>
        str.Replace("Attribute", string.Empty).ToLowerInvariant();
    
    public static string ToLowerNoAttributePostfix(this AttributeSyntax attribute) 
        => attribute.Name.ToString().ToLowerNoAttributePostfix();
    
    public static IEnumerable<AttributeArgumentSyntax> GetArguments(this AttributeSyntax attribute)
        => attribute.ArgumentList?.Arguments ?? Enumerable.Empty<AttributeArgumentSyntax>();
}