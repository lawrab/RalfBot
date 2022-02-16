using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Infrastrtucture
{
    public static class StorageHelper
    {
        public static async Task<IStorageProvider<LeagueStorageProviderModel>> CreateLeaguesStorage(string dataPath)
        {
            var fileStorageProvider = new JsonFileStorageProvider(dataPath);
            var storageProvider = new StorageProvider<LeagueStorageProviderModel>();
            await storageProvider.SetFileStorageProvider(fileStorageProvider);

            return storageProvider;
        }

        public static async Task<IStorageProvider<RolesStorageProviderModel>> CreateRoleStorage(string dataPath)
        {
            var fileStorageProvider = new JsonFileStorageProvider(dataPath);
            var storageProvider = new StorageProvider<RolesStorageProviderModel>();
            await storageProvider.SetFileStorageProvider(fileStorageProvider);

            return storageProvider;
        }

        public static async Task<IStorageProvider<NewsStorageProviderModel>> CreateNewsStorage(string dataPath)
        {
            var fileStorageProvider = new JsonFileStorageProvider(dataPath);
            var storageProvider = new StorageProvider<NewsStorageProviderModel>();
            await storageProvider.SetFileStorageProvider(fileStorageProvider);

            return storageProvider;
        }
    }
}
