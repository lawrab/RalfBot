using Microsoft.Extensions.Logging;
using SnailRacing.Store;

namespace SnailRacing.Ralf.Providers
{
    // only supports JsonStore at the moment
    public class StorageProvider : IStorageProvider
    {
        private readonly JsonStore<string> _store;
        private readonly string _rootPath;
        private readonly ILogger<StorageProvider> _logger;

        private StorageProvider(string rootPath, ILogger<StorageProvider> logger)
        {
            _rootPath = rootPath;
            _logger = logger;

            _store = new(Path.Combine(rootPath, "storage.json"));
        }

        public static StorageProvider Create(string rootPath, ILogger<StorageProvider> logger)
        {
            var storageProvider = new StorageProvider(rootPath, logger);
            storageProvider._store.Init().Wait();
            return storageProvider;
        }

        public void Add(string group, string key)
        {
            string path = GetFilePath(group, key);
            string folderPath = GetFolderPath(group);
            string storeKey = GetStoreKey(group, key);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            _store.TryAdd(storeKey, path);
        }
        public void Add(string key)
        {
            Add(string.Empty, key);
        }

        public IStore<TModel> Get<TModel>(string key)
        {
            return Get<TModel>(string.Empty, key);
        }

        public IStore<TModel> Get<TModel>(string group, string key)
        {
            var storeKey = GetStoreKey(group, key);
            var store = new JsonStore<TModel>(_store[storeKey]);
            store.Init().Wait();
            return store;
        }

        public bool Contains(string group, string key)
        {
            var storeKey = GetStoreKey(group, key);
            return _store.ContainsKey(storeKey);
        }

        private static string GetStoreKey(string group, string key)
        {
            return $"{group}_{key}";
        }

        private string GetFolderPath(string group)
        {
            return Path.Combine(_rootPath, group);
        }

        private string GetFilePath(string group, string key)
        {
            return Path.Combine(_rootPath, group, $"{key}.json");
        }
    }
}
