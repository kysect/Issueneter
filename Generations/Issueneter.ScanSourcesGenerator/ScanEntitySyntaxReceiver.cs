using Issueneter.Annotation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Issueneter.ScanSourcesGenerator;

internal class ScanEntitySyntaxReceiver : ISyntaxReceiver
{
    private const string TriggerInterface = nameof(IFilterable);

    public List<ScanEntity> Entities { get; } = new();
    
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not ClassDeclarationSyntax classDeclaration)
            return;

        var bases = classDeclaration.BaseList?.Types;

        if (bases is not {Count: >0} baseTypes)
            return;


        if (!baseTypes.Any(k => k.Type.ToString().Equals(TriggerInterface, StringComparison.OrdinalIgnoreCase)))
            return;

        var entity = classDeclaration;
        
        Entities.Add(new ScanEntity(entity));
    }
}