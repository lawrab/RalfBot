using System.Collections;
using System.Collections.Concurrent;
using System.Text.Json;

namespace SnailRacing.Store
{
    public class JsonStore<TEntity> : IStore<TEntity>
    {
        private ConcurrentDictionary<string, TEntity> _data = new(StringComparer.InvariantCultureIgnoreCase);
        private readonly string _filePath;

        public JsonStore(string filePath)
        {
            _filePath = filePath;
        }

        public TEntity this[string key]
        {
            get
                {
                var canGet = _data.TryGetValue(key, out var value);

                if (!canGet) throw new KeyNotFoundException($"{key} not found in Store");

                return value;
            }
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
            var data = JsonSerializer.Deserialize<Dictionary<string, TEntity>>(jsonStr);
            if (data != null)
            {
                _data = new ConcurrentDictionary<string, TEntity>(data);
            }
        }

        public bool TryAdd(string key, TEntity? value)
        {
            var res = _data.TryAdd(key, value);
            if (res) SaveData();

            return res;
        }

        public bool TryRemove(string key)
        {
            var res = _data.TryRemove(key, out _);
            if (res) SaveData();

            return res;
        }

        private void SaveData()
        {
            var json = JsonSerializer.Serialize(_data);
            File.WriteAllText(_filePath, json);
        }

        public IEnumerator<KeyValuePair<string, TEntity>> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public bool TryUpdate(string key, TEntity? newValue)
        {
            var currentValue = _data[key];
            var res = _data.TryUpdate(key, newValue, currentValue);

            if (res) SaveData();

            return res;
        }

        public bool ContainsKey(string key)
        {
            return _data.ContainsKey(key);
        }
    }
}
