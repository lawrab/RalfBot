namespace SnailRacing.Store
{
    public interface IStore<TKey, TValue>
    {
        bool TryAdd(TKey key, TValue value);
        bool TryRemove(TKey key);
        Task Init();
        TValue this[TKey key] { get; }
    }
}
