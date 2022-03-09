using SnailRacing.Ralf.Providers;
using SnailRacing.Store;

namespace SnailRacing.Ralf.Infrastrtucture
{
    public static class StoreHelper
    {
        private static IStore<TKey, TValue> GetStore<TKey, TValue>(string key, string guildId, IStorageProvider storageProvider)
        {
            return storageProvider.Get<IStore<TKey, TValue>>(new StoreKey(guildId, key));
        }

        public static IStore<string, NewsModel> GetNewsStore(string guildId, IStorageProvider storageProvider)
        {
            return GetStore<string, NewsModel>(StorageProviderConstants.NEWS, guildId, storageProvider);
        }

        public static IStore<string, string> GetRolesStore(string guildId, IStorageProvider storageProvider)
        {
            return GetStore<string, string>(StorageProviderConstants.ROLES, guildId, storageProvider);
        }

        public static IStore<string, LeagueModel> GetLeagueStore(string guildId, IStorageProvider storageProvider)
        {
            return GetStore<string, LeagueModel>(StorageProviderConstants.LEAGUE, guildId, storageProvider);
        }
    }
}
