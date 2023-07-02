namespace Issueneter.ScanSourcesGenerator;

public class ModelProperties
{
    public string Name { get;}
    public ModelProperty[] Properties { get; }

    public ModelProperties(string name, ModelProperty[] properties)
    {
        Name = name;
        Properties = properties;
    }
}