using Microsoft.Extensions.Logging;
using SnailRacing.Store;

namespace SnailRacing.Ralf.Providers
{
    public class StorageProvider : IStorageProvider
    {
        private readonly JsonStore<StoreKey, object> _store;
        private readonly string _rootPath;
        private readonly ILogger<StorageProvider> _logger;

        public StorageProvider(string rootPath, ILogger<StorageProvider> logger)
        {
            _rootPath = rootPath;
            _logger = logger;

            _store = new(rootPath);
        }

        public object this[StoreKey key]
        {
            get => _store;
        }

        public TModel Get<TModel>(StoreKey key)
        {
            return (TModel)_store[key];
        }

        public void Add<TModel>(StoreKey key, TModel value)
        {
            _store.TryAdd(key, value);
        }
    }
}
