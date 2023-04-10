namespace Issueneter.Annotation;

[AttributeUsage(AttributeTargets.Property)]
public class ScanPublicAttribute : Attribute
{
    public ScanPublicAttribute(string? overrodeName = null)
    {
        OverrodeName = overrodeName;
    }

    public string? OverrodeName { get; }
}