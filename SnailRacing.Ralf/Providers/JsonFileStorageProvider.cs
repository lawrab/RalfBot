using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace SnailRacing.Ralf.Providers
{
    [ExcludeFromCodeCoverage(Justification = "File IO implementation that cannot be tested predictably")]
    public class JsonFileStorageProvider : IJsonFileStorageProvider
    {
        private readonly string _filePath;

        public JsonFileStorageProvider(string filePath)
        {
            this._filePath = filePath;
        }

        public string FilePath => _filePath;

        public async Task<T?> LoadAsync<T>()
        {
            try
            {
                var json = await File.ReadAllTextAsync(FilePath);

                return JsonSerializer.Deserialize<T>(json);
            }
            catch (FileNotFoundException)
            {
                return default(T);
            }
        }

        public async Task<object?> LoadAsync(Type type)
        {
            try
            {
                var json = await File.ReadAllTextAsync(FilePath);

                return JsonSerializer.Deserialize(json, type);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public async Task SaveAsync<T>(T memoryStore)
        {
            var json = JsonSerializer.Serialize(memoryStore);
            await File.WriteAllTextAsync(FilePath, json);
        }
    }
}
