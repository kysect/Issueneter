using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Issueneter.ScanSourcesGenerator.Extensions;

public static class ClassDeclarationExtensions
{
    public static string GetClassName(this ClassDeclarationSyntax syntax) => syntax.Identifier.ToFullString();

    public static IEnumerable<PropertyDeclarationSyntax> GetProperties(this ClassDeclarationSyntax syntax)
        => syntax.DescendantNodes().OfType<PropertyDeclarationSyntax>();
}