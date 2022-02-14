namespace SnailRacing.Ralf.Providers
{
    public interface IJsonFileStorageProvider
    {
        Task<T?> LoadAsync<T>();

        Task<object?> LoadAsync(Type type);

        Task SaveAsync<T>(T store);
    }
}
