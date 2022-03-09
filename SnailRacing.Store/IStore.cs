namespace SnailRacing.Store
{
    public interface IStore<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        bool TryAdd(TKey key, TValue? value);
        bool TryUpdate(TKey key, TValue? newValue);
        bool TryRemove(TKey key);
        Task Init();
        TValue this[TKey key] { get; }
    }
}
