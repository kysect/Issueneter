using Issueneter.Mappings;

namespace Issueneter.Annotation;

public interface IFilterable
{
    string GetProperty(string name);

    static abstract ScanType ScanType { get; }
}