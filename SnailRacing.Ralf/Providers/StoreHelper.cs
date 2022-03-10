using SnailRacing.Ralf.Providers;
using SnailRacing.Store;

namespace SnailRacing.Ralf.Infrastrtucture
{
    public static class StoreHelper
    {
        private static IStore<TValue> GetStore<TValue>(string key, string guildId, IStorageProvider storageProvider)
        {
            return storageProvider.Get<TValue>(guildId, key);
        }

        public static IStore<NewsModel> GetNewsStore(string guildId, IStorageProvider storageProvider)
        {
            return GetStore<NewsModel>(StorageProviderConstants.NEWS, guildId, storageProvider);
        }

        public static IStore<string> GetRolesStore(string guildId, IStorageProvider storageProvider)
        {
            return GetStore<string>(StorageProviderConstants.ROLES, guildId, storageProvider);
        }

        public static IStore<LeagueModel> GetLeagueStore(string guildId, IStorageProvider storageProvider)
        {
            return GetStore<LeagueModel>(StorageProviderConstants.LEAGUE, guildId, storageProvider);
        }

        public static LeagueModel GetLeague(string guildId, string leagueKey, IStorageProvider storageProvider)
        {
            var store = GetStore<LeagueModel>(StorageProviderConstants.LEAGUE, guildId, storageProvider);
            return store[leagueKey];
        }
    }
}
