namespace Issueneter.ScanSourcesGenerator;

public class FilterableInterfaceGenerationHelper
{
    public const string InterfaceName = "IFilterable";

    public static string Generate()
    {
        return $$"""
                using Issueneter.Mappings;

                namespace Issueneter.Annotation;

                public interface {{InterfaceName}}
                {
                    string GetProperty(string name);

                    static abstract ScanType ScanType { get; }
                }
                """;
    }
}