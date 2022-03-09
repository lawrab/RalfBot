using System.Collections;
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

        public IEnumerator<KeyValuePair<TKey, TEntity>> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public Task Init()
        {
            return Task.CompletedTask;
        }

        public bool TryAdd(TKey key, TEntity value)
        {
            return _data.TryAdd(key, value);
        }

        public bool TryRemove(TKey key)
        {
            return _data.TryRemove(key, out _);
        }

        public bool TryUpdate(TKey key, TEntity? newValue)
        {
            var currentValue = _data[key];
            return _data.TryUpdate(key, newValue, currentValue);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}
