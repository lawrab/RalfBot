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
        private string _jsonStoreRootPath { get; set; } = string.Empty;

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

        public IStore<TValue> Build<TKey, TValue>() 
            where TKey : notnull
            where TValue : new() =>
            
            _storeType switch
            {
                StoryType.Memory => new MemoryStore<TValue>(),
                StoryType.Json => new JsonStore<TValue>(_jsonStoreRootPath),
                _ => throw new InvalidOperationException("Type of store not specified")
            };
    }
}
