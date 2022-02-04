using System.Collections.Concurrent;
using System.Text.Json;

namespace SnailRacing.Ralf.Providers
{
    /// <summary>
    /// Generic storage provider in a dictionary structure
    /// Data is held in memory and persisted to file to load at startup
    /// </summary>
    public class StorageProvider<TKey, TValue> : IStorageProvider<TKey, TValue>
        where TKey: notnull
    {
        private ConcurrentDictionary<TKey, TValue> memoryStore = new ConcurrentDictionary<TKey, TValue>();
        private IJsonFileStorageProvider<ConcurrentDictionary<TKey, TValue>>? fileStorageProvider;

        public StorageProvider()
        {

        }

        public async Task SetFileStorageProvider(IJsonFileStorageProvider<ConcurrentDictionary<TKey, TValue>> fileStorageProvider)
        {
            this.fileStorageProvider = fileStorageProvider;
            var store = await fileStorageProvider.LoadAsync();
            
            if (store is null) return;
            
            this.memoryStore = store;
        }

        public TValue this[TKey key] 
        { 
            get => memoryStore[key];
            set => UpdateStore(key, value);
        }

        private void UpdateStore(TKey key, TValue value)
        {
            memoryStore[key] = value;
            SaveData().ConfigureAwait(false);
        }

        private async Task SaveData()
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(memoryStore);

                if (fileStorageProvider is null) return;
                await fileStorageProvider.SaveAsync(memoryStore);
            }
            catch
            {
                // Error occured
            }
        }
    }
}
