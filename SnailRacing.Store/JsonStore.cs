using System.Collections.Concurrent;
using System.Text.Json;

namespace SnailRacing.Store
{
    public class JsonStore<TKey, TEntity> : IStore<TKey, TEntity>
        where TKey : notnull
    {
        private readonly ConcurrentDictionary<TKey, TEntity> _data = new();
        private readonly string _rootPath;

        public JsonStore(string _filePath)
        {
            this._rootPath = _filePath;
        }

        public TEntity this[TKey key]
        {
            get => _data[key];
        }

        public bool TryAdd(TKey key, TEntity value)
        {
            var res = _data.TryAdd(key, value);
            if (res) SaveData();

            return res;
        }

        public bool TryRemove(TKey key)
        {
            var res = _data.TryRemove(key, out _);
            if (res) SaveData();

            return res;
        }

        private void SaveData()
        {
            var json = JsonSerializer.Serialize(_data);
            File.WriteAllTextAsync(_rootPath, json).Wait();
        }
    }
}
