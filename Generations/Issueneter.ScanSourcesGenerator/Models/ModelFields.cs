namespace Issueneter.ScanSourcesGenerator;

public class ModelProperties
{
    public string Name { get;}
    public string[] Properties { get; }

    public ModelProperties(string name, string[] properties)
    {
        Name = name;
        Properties = properties;
    }
}