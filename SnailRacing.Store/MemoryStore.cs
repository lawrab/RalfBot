using System.Collections.Concurrent;

namespace SnailRacing.Store
{
    public class MemoryStore<TKey, TEntity> : IStore<TKey, TEntity>
        where TKey : notnull
    {
        private readonly ConcurrentDictionary<TKey, TEntity> _data = new();

        public TEntity this[TKey key]
        {
            get => _data[key];
        }

        public bool TryAdd(TKey key, TEntity value)
        {
            return _data.TryAdd(key, value);
        }

        public bool TryRemove(TKey key)
        {
            return _data.TryRemove(key, out _);
        }
    }
}
