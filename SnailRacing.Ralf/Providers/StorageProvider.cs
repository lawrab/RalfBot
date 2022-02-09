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
        private StorageProviderModel<TKey, TValue> memoryStore = new StorageProviderModel<TKey, TValue>();
        private IJsonFileStorageProvider<StorageProviderModel<TKey, TValue>>? fileStorageProvider;

        public StorageProvider()
        {

        }

        public async Task SetFileStorageProvider(IJsonFileStorageProvider<StorageProviderModel<TKey, TValue>> fileStorageProvider)
        {
            this.fileStorageProvider = fileStorageProvider;
            var store = await fileStorageProvider.LoadAsync();
            
            if (store is null) return;
            
            this.memoryStore = store;
        }

        public Dictionary<string, string> SyncRoles => memoryStore.SyncRoles;

        public void AddRole(string source, string target)
        {
            memoryStore.SyncRoles[source] = target;
            SaveData().ConfigureAwait(false);
        }

        public TValue? this[TKey key] 
        { 
            // ToDo: fix nullability warnings here!!!!!!!
            get 
            {
                TValue value;
                var hasValue = memoryStore.Props.TryGetValue(key, out value!);
                return hasValue ? value : default(TValue);
            }
            set => UpdateStore(key, value!);
        }

        private void UpdateStore(TKey key, TValue value)
        {
            memoryStore.Props[key] = value;
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

        public void RemoveRole(string role)
        {
            memoryStore.SyncRoles.Remove(role);
        }

        public void Remove(TKey key)
        {
            TValue value;
            memoryStore.Props.Remove(key, out value!);
        }
    }
}
