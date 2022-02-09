namespace SnailRacing.Ralf.Providers
{
    public interface IStorageProvider<TKey, TValue>
        where TKey: notnull
    {
        TValue this[TKey key] { get; set; }
        Dictionary<string, string> SyncRoles { get;}
        void AddRole(string source, string target);
        void RemoveRole(string role);
        void Remove(TKey key);
    }
}
