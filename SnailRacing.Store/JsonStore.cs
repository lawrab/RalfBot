using System.Collections.Concurrent;

namespace SnailRacing.Store
{
    public class JsonStore<TKey, TEntity> : ConcurrentDictionary<TKey, TEntity>, IStore<TKey, TEntity>
        where TKey : notnull
    {

    }
}
