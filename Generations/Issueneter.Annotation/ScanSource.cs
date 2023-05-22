namespace Issueneter.Annotation;

public class ScanSource
{
    private readonly List<string> _fields;

    public ScanSource Source { get; }
    public IReadOnlyCollection<string> Fields  => _fields;

    public ScanSource(ScanSource source, List<string> fields)
    {
        Source = source;
        _fields = fields;
    }
}