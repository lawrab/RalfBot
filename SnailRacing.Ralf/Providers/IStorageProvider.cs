namespace SnailRacing.Ralf.Providers
{
    public interface IStorageProvider<TModel>
    {
        public TModel Store { get; }
    }
}
