using System.Collections.Concurrent;

namespace SnailRacing.Store
{
    public class MemoryStore<TKey, TEntity> : ConcurrentDictionary<TKey, TEntity>, IStore<TKey, TEntity>
        where TKey : notnull
    {
        public void Init()
        {
        }
    }
}
