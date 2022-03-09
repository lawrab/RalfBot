namespace SnailRacing.Ralf.Providers
{
    public interface IStorageProvider
    {
        object this[StoreKey key] { get; }
        void Add<TModel>(StoreKey key, TModel value);
        TModel Get<TModel>(StoreKey key);
    }
}
