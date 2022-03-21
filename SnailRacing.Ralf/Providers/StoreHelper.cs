using SnailRacing.Ralf.Providers;
using SnailRacing.Store;

namespace SnailRacing.Ralf.Infrastrtucture
{
    public static class StoreHelper
    {
        private static IStore<TValue> GetStore<TValue>(string key, string guildId, IStorageProvider storageProvider)
        {
            // ToDo: refactor this, create the store if it does not exist yet
            if (!storageProvider.Contains(guildId, key))
            {
                storageProvider.Add(guildId, key); 
            }
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

        public static GuildConfigModel GetGuildConfig(string guildId, IStorageProvider? storageProvider)
        {
            if (storageProvider is null) throw new ArgumentException($"{nameof(storageProvider)} cannot be null");

            var store = GetStore<GuildConfigModel>(StorageProviderConstants.GUILD_CONFIG, guildId, storageProvider);

            if(!store.ContainsKey(guildId))
            {
                store.TryAdd(guildId, new GuildConfigModel { GuildId = guildId });
            }

            return store[guildId];
        }

        public static IStore<GuildConfigModel> GetGuildConfigStore(string guildId, IStorageProvider? storageProvider)
        {
            if (storageProvider is null) throw new ArgumentException($"{nameof(storageProvider)} cannot be null");

            var store = GetStore<GuildConfigModel>(StorageProviderConstants.GUILD_CONFIG, guildId, storageProvider);

            if (!store.ContainsKey(guildId))
            {
                store.TryAdd(guildId, new GuildConfigModel { GuildId = guildId });
            }

            return store;
        }
    }
}
