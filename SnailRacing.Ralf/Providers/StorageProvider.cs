using Microsoft.Extensions.Logging;

namespace SnailRacing.Ralf.Providers
{
    public class StorageProvider<TModel> : IStorageProvider<TModel>
        where TModel : IStorageProviderModel, new()
    {
        private readonly ILogger<StorageProvider<TModel>>? _logger;
        private TModel _model = new TModel();
        private IJsonFileStorageProvider<TModel>? _fileStorageProvider;

        public StorageProvider()
        {
            _model.SetSaveDataCallback(SaveData);
        }

        public StorageProvider(ILogger<StorageProvider<TModel>> logger)
            : this()
        {
            _logger = logger;
            _logger?.LogDebug($"Logger set for {this.GetType().Name}");
        }

        public TModel Store
        {
            get => _model;
        }

        public async Task SetFileStorageProvider(IJsonFileStorageProvider<TModel> fileStorageProvider)
        {
            _fileStorageProvider = fileStorageProvider;
            var data = await fileStorageProvider.LoadAsync();

            if (data is null) return;

            _model = data;
            _model.SetSaveDataCallback(SaveData);
        }

        private void SaveData()
        {
            if (_fileStorageProvider is null) return;
            _fileStorageProvider.SaveAsync(_model)
            .ContinueWith(t => _logger?.LogError(t.Exception, "Error persisting StorageProvider memoryStore"),
                                TaskContinuationOptions.OnlyOnFaulted); ;
        }
    }
}
