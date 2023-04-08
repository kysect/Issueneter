namespace Issueneter.Persistence;

public class ScanEntry
{
    public ScanEntry(){}
    public ScanEntry(long Id, short ScanType, string Owner, string Repo, long ChatId, DateTime Created, string Filters)
    {
        this.Id = Id;
        this.ScanType = ScanType;
        this.Owner = Owner;
        this.Repo = Repo;
        this.ChatId = ChatId;
        this.Created = Created;
        this.Filters = Filters;
    }

    public long Id { get; init; }
    public short ScanType { get; init; }
    public string Owner { get; init; }
    public string Repo { get; init; }
    public long ChatId { get; init; }
    public DateTime Created { get; init; }
    public string Filters { get; init; }
}