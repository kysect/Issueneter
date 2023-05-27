using System.Text;

namespace Issueneter.ScanSourcesGenerator;

public static class ScanTypeGenerationHelper
{
    private const string Start = """
        namespace Issueneter.Mappings;
        
        public readonly struct ScanType
        {

        """;

    private const string Middle = """
            public static ScanType Parse(string value)
            {
                return value.ToLower() switch
                {

        """;
    
    private const string End = """
                    _ => throw new ArgumentOutOfRangeException(nameof(value))
                };
            }
        
            public string Value { get; }
        
            private ScanType(string value)
            {
                Value = value;
            }
        
            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj.GetType() == this.GetType() && Equals((ScanType)obj);
            }
        
            private bool Equals(ScanType other)
            {
                return Value == other.Value;
            }
        
            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }
            
            public static bool operator ==(string left, ScanType right)
            {
                return string.Equals(left, right.Value, StringComparison.InvariantCultureIgnoreCase);
            }
        
            public static bool operator !=(string left, ScanType right)
            {
                return !(left == right);
            }
        
            public static bool operator ==(ScanType left, ScanType right)
            {
                return left.Equals(right);
            }
        
            public static bool operator !=(ScanType left, ScanType right)
            {
                return !(left == right);
            }
        }
        """;
    
    private static string WrapWithQuotes(string str) => $"\"{str}\"";

    public static string Generate(IEnumerable<ModelProperties> sources)
    {
        var builder = new StringBuilder();
        builder.Append(Start);
        foreach (var model in sources)
        {
            builder.AppendLine($"\tpublic static ScanType {model.Name} => new ScanType({WrapWithQuotes(model.Name)});");
        }

        builder.Append(Middle);
        
        foreach (var model in sources)
        {
            builder.AppendLine($"\t\t\t{WrapWithQuotes(model.Name.ToLower())} => {model.Name},");
        }

        builder.Append(End);

        return builder.ToString();
    }
}