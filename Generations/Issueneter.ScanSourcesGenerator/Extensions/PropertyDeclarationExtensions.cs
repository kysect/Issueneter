using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Issueneter.ScanSourcesGenerator.Extensions;

public static class PropertyDeclarationExtensions
{
    public static IEnumerable<AttributeSyntax> GetAttributes(this PropertyDeclarationSyntax prop)
        => prop.AttributeLists.SelectMany(k => k.Attributes);

    public static string GetName(this PropertyDeclarationSyntax prop)
        => prop.Identifier.ToFullString();
}