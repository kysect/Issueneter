using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Issueneter.ScanSourcesGenerator;

[Generator]
public class AvailableScanSourcesGenerator : ISourceGenerator
{
    private ScanSourcesCodeGenerator _generator = new ScanSourcesCodeGenerator();
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new ScanEntitySyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if(context.SyntaxReceiver is not ScanEntitySyntaxReceiver receiver)
            return;

        foreach (var entity in receiver.Entities)
        {
            var results = ScanProcessing.ToAvailableProperties(entity);
            _generator.AddEntity(results.Entity, results.Properties);
        }

        context.AddSource("Generated.g.cs", SourceText.From(_generator.GetResult(), Encoding.UTF8));
    }
}