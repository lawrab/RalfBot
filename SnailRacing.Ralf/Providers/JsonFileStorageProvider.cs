using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace SnailRacing.Ralf.Providers
{
    [ExcludeFromCodeCoverage(Justification = "File IO implementation that cannot be tested predictably")]
    public class JsonFileStorageProvider<T> : IJsonFileStorageProvider<T>
        where T : new()
    {
        private readonly string filePath;

        public JsonFileStorageProvider(string filePath)
        {
            this.filePath = filePath;
        }

        public async Task<T?> LoadAsync()
        {
            try
            {
                var json = await File.ReadAllTextAsync(filePath);

                return JsonSerializer.Deserialize<T>(json);
            }
            catch (FileNotFoundException)
            {
                return new T();
            }
        }

        public async Task SaveAsync(T memoryStore)
        {
            var json = JsonSerializer.Serialize(memoryStore);
            await File.WriteAllTextAsync(filePath, json);
        }
    }
}
