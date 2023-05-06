using Issueneter.Annotation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Issueneter.ScanSourcesGenerator;

internal class ScanEntitySyntaxReceiver : ISyntaxReceiver
{
    private const string TriggerInterface = nameof(IFilterable);
    private readonly List<ClassDeclarationSyntax> _entities = new();

    public IReadOnlyList<ClassDeclarationSyntax> Entities => _entities;
    
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not ClassDeclarationSyntax classDeclaration)
            return;

        var bases = classDeclaration.BaseList?.Types;

        if (bases is not {Count: >0} baseTypes)
            return;


        if (!baseTypes.Any(k => k.Type.ToString().Equals(TriggerInterface, StringComparison.OrdinalIgnoreCase)))
            return;

        _entities.Add(classDeclaration);
    }
}