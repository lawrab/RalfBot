namespace SnailRacing.Store
{
    public class StoreBuilder
    {
        private enum StoryType
        {
            NotSet,
            Json,
            Memory

        }

        private StoryType _storeType;
        private string _jsonStoreRootPath { get; set; }

        public StoreBuilder Init()
        {
            return new StoreBuilder();
        }

        public StoreBuilder WithJsonStore(string rootPath)
        {
            _storeType = StoryType.Json;
            _jsonStoreRootPath = rootPath;
            return this;
        }

        public StoreBuilder WithMemoryStore()
        {
            _storeType = StoryType.Memory;
            return this;
        }

        public IStore<TKey, TValue> Build<TKey, TValue>() where TKey : notnull =>
            
            _storeType switch
            {
                StoryType.Memory => new MemoryStore<TKey, TValue>(),
                StoryType.Json => new JsonStore<TKey, TValue>(_jsonStoreRootPath),
                _ => throw new InvalidOperationException("Type of store not specified")
            };
    }
}
