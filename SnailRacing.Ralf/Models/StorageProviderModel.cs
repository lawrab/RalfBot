using System.Collections.Concurrent;

namespace SnailRacing.Ralf.Models
{
    public class StorageProviderModel<TKey, TValue>
        where TKey : notnull
    {
        public Dictionary<string, string> SyncRoles { get; set; } = new Dictionary<string, string>();
        public ConcurrentDictionary<TKey, TValue> Props { get; set; } = new ConcurrentDictionary<TKey, TValue>();
    }
}
