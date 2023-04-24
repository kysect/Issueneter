using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Issueneter.ScanSourcesGenerator;

internal class ScanEntity
{
    public ScanEntity(ClassDeclarationSyntax entity)
    {
        Entity = entity;
    }
    
    public ClassDeclarationSyntax Entity { get; }
}