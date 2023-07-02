namespace Issueneter.Annotation;

[AttributeUsage(AttributeTargets.Property)]
public class ScanPropertyAttribute : Attribute
{
    public ScanPropertyAttribute(string? overrodeName = null)
    {
        OverrodeName = overrodeName;
    }

    public string? OverrodeName { get; }
}