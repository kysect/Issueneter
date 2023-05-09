using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using Issueneter.Annotation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Issueneter.ScanSourcesGenerator;

[Generator]
public class ScanSourcesGenerator : IIncrementalGenerator
{
    private const string TriggerInterface = nameof(IFilterable);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var filterables = context
            .SyntaxProvider
            .CreateSyntaxProvider(predicate: FilterSyntaxNode, transform: TransformSyntaxNode)
            .Where(x => x is not null)
            .Collect();

        var combinedContext = context.CompilationProvider.Combine(filterables);
        context.RegisterSourceOutput(combinedContext, static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    private static bool FilterSyntaxNode(SyntaxNode syntaxNode, CancellationToken cancellationToken = default)
    {
        if (syntaxNode is not ClassDeclarationSyntax classDeclaration)
            return false;

        var bases = classDeclaration.BaseList?.Types;

        if (bases is not {Count: >0} baseTypes)
            return false;


        return baseTypes.Any(k => k.Type.ToString().Equals(TriggerInterface, StringComparison.OrdinalIgnoreCase));
    }

    private static ModelProperties TransformSyntaxNode(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        if (context.Node is not ClassDeclarationSyntax classNode)
            return null;
        return ScanProcessing.ToAvailableProperties(classNode);
    }
    
    private static void Execute(Compilation compilation, ImmutableArray<ModelProperties> models, SourceProductionContext context)
    {
        var code = ScanSourcesGenerationHelper.Generate(models);
        context.AddSource("ModelsInfo.g.cs", SourceText.From(code, Encoding.UTF8));
    }
}