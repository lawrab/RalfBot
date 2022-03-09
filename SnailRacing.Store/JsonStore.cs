using System.Collections;
using System.Collections.Concurrent;
using System.Text.Json;

namespace SnailRacing.Store
{
    public class JsonStore<TKey, TEntity> : IStore<TKey, TEntity>
        where TKey : notnull
    {
        private ConcurrentDictionary<TKey, TEntity> _data = new();
        private readonly string _filePath;

        public JsonStore(string filePath)
        {
            _filePath = filePath;
        }

        public TEntity this[TKey key]
        {
            get => _data[key];
        }

        public async Task Init()
        {
            if(File.Exists(_filePath))
            {
                await LoadData(_filePath);
            }
        }

        private async Task LoadData(string filePath)
        {
            var jsonStr = await File.ReadAllTextAsync(filePath);
            var data = JsonSerializer.Deserialize<Dictionary<TKey, TEntity>>(jsonStr);
            if (data != null)
            {
                _data = new ConcurrentDictionary<TKey, TEntity>(data);
            }
        }

        public bool TryAdd(TKey key, TEntity? value)
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
            File.WriteAllTextAsync(_filePath, json).Wait();
        }

        public IEnumerator<KeyValuePair<TKey, TEntity>> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public bool TryUpdate(TKey key, TEntity? newValue)
        {
            var currentValue = _data[key];
            return _data.TryUpdate(key, newValue, currentValue);
        }
    }
}
