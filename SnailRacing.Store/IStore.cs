namespace SnailRacing.Store
{
    public interface IStore<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey : notnull
    {
        void Init();
    }
}
