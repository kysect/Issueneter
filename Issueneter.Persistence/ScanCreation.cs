namespace Issueneter.Persistence;

// TODO: Засурсгенить
public readonly struct ScanType
{
    public static ScanType Issue => new ScanType("Issue");
    public static ScanType PullRequest => new ScanType("PullRequest");
    
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
}

public record ScanCreation(ScanType Type, string Owner, string Repo, long ChatId, string Filters);