namespace SnailRacing.Ralf.Providers
{
    internal interface IStorageProvider<TKey, TValue>
        where TKey: notnull
    {
        TValue this[TKey key] { get; set; }
    }
}
