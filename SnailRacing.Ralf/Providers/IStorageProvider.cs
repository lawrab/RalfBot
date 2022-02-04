namespace SnailRacing.Ralf.Providers
{
    public interface IStorageProvider<TKey, TValue>
        where TKey: notnull
    {
        TValue this[TKey key] { get; set; }
    }
}
