namespace SnailRacing.Ralf.Providers
{
    public interface IJsonFileStorageProvider<T>
    {
        Task<T?> LoadAsync();

        Task SaveAsync(T memoryStore);
    }
}
