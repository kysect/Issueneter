namespace Issueneter.ScanSourcesGenerator;

public class ModelProperty
{
    public string Name { get; }
    public string FieldName { get; }

    public ModelProperty(string name, string fieldName)
    {
        Name = name;
        FieldName = fieldName;
    }
}