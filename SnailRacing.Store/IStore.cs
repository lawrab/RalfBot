namespace SnailRacing.Store
{
    public interface IStore<TValue> : IEnumerable<KeyValuePair<string, TValue>>
    {
        bool TryAdd(string key, TValue? value);
        bool TryUpdate(string key, TValue? newValue);
        bool TryRemove(string key);
        Task Init();
        TValue this[string key] { get; }
    }
}
