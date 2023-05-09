namespace Issueneter.Annotation;

public class ScanSource
{
    private readonly List<string> _fields;

    public string Name { get; }
    public IReadOnlyCollection<string> Fields  => _fields;

    public ScanSource(string name, List<string> fields)
    {
        Name = name;
        _fields = fields;
    }
}