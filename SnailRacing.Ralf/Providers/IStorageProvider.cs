namespace SnailRacing.Ralf.Providers
{
    public interface IStorageProvider<TModel>
        where TModel : IStorageProviderModel
    {
        public TModel Store { get; }
    }
}
