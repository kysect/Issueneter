using Issueneter.Annotation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Issueneter.ScanSourcesGenerator;

public class ScanEntity
{
    public ScanEntity(ClassDeclarationSyntax entity, List<PropertyDeclarationSyntax> defaults, List<PropertyDeclarationSyntax> overrides, List<PropertyDeclarationSyntax> ignores)
    {
        Entity = entity;
        Defaults = defaults;
        Overrides = overrides;
        Ignores = ignores;
    }

    public ClassDeclarationSyntax Entity { get; }
    public List<PropertyDeclarationSyntax> Defaults { get; }
    public List<PropertyDeclarationSyntax> Overrides { get; }
    public List<PropertyDeclarationSyntax> Ignores { get; }
}

public class ScanEntitySyntaxReceiver : ISyntaxReceiver
{
    private const string TriggerInterface = nameof(IFilterable);
    private const string ScanPublicTrigger = nameof(ScanPublicAttribute);
    private const string ScanInternalTrigger = nameof(ScanInternalAttribute);

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
        var ignores = new List<PropertyDeclarationSyntax>();
        var overrides = new List<PropertyDeclarationSyntax>();
        var defaults = new List<PropertyDeclarationSyntax>();
        
        var properties = classDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>();

        foreach (var property in properties)
        {
            var attributes = property
                .AttributeLists
                .SelectMany(k => k.Attributes)
                .Select(k => k.Name.ToString());
            
            var attrNames = new HashSet<string>(attributes);
            
            if (attrNames.Contains(ScanPublicTrigger, StringComparer.OrdinalIgnoreCase))
                overrides.Add(property);
            else if(attrNames.Contains(ScanInternalTrigger, StringComparer.OrdinalIgnoreCase))
                ignores.Add(property);
            else
                defaults.Add(property);
        }

        Entities.Add(new ScanEntity(entity, defaults, overrides, ignores));
    }
}