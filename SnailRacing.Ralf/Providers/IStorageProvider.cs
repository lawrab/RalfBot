using SnailRacing.Store;

namespace SnailRacing.Ralf.Providers
{
    public interface IStorageProvider
    {
        void Add(string key);
        void Add(string group, string key);
        IStore<TModel> Get<TModel>(string key);
        IStore<TModel> Get<TModel>(string group, string key);
        bool Contains(string group, string key);
    }
}
