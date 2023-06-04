namespace Issueneter.ScanSourcesGenerator;

public class FilterableInterfaceGenerationHelper
{
    public const string InterfaceName = "IFilterable";

    public static string Generate()
    {
        return $$"""
                namespace Issueneter.Annotation;

                public interface {{InterfaceName}}
                {
                    string GetProperty(string name);
                }
                """;
    }
}