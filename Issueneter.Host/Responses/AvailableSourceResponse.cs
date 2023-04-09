namespace Issueneter.Host.Responses;

public class AvailableSourceResponse
{
    public AvailableSourceResponse(string name, string field)
    {
        Name = name;
        Field = field;
    }

    public string Name { get; }
    public string Field { get; }
}