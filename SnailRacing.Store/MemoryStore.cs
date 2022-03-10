using System.Collections;
using System.Collections.Concurrent;

namespace SnailRacing.Store
{
    public class MemoryStore<TEntity> : IStore<TEntity>
    {
        private readonly ConcurrentDictionary<string, TEntity> _data = new();

        public TEntity this[string key]
        {
            get => _data[key];
        }

        public IEnumerator<KeyValuePair<string, TEntity>> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public Task Init()
        {
            return Task.CompletedTask;
        }

        public bool TryAdd(string key, TEntity value)
        {
            return _data.TryAdd(key, value);
        }

        public bool TryRemove(string key)
        {
            return _data.TryRemove(key, out _);
        }

        public bool TryUpdate(string key, TEntity? newValue)
        {
            var currentValue = _data[key];
            return _data.TryUpdate(key, newValue, currentValue);
        }
        public bool ContainsKey(string key)
        {
            return _data.ContainsKey(key);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}
