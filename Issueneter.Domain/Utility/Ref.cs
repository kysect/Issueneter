namespace Issueneter.Domain.Utility;

public class Ref<T>
{
    private readonly Func<Task<T>> _loader;
    private T? _value;

    public Ref(Func<Task<T>> loader)
    {
        _loader = loader;
    }

    public bool IsLoaded { get; private set; }

    //TODO: Synchronization
    public async Task<T> Load()
    {
        if (IsLoaded) return _value;
        _value = await _loader();
        IsLoaded = true;
        return _value;
    }
}