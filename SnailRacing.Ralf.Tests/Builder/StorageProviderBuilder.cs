using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
using SnailRacing.Store;
using System;
using System.Collections.Generic;

namespace SnailRacing.Ralf.Tests.Builder
{
    internal class StorageProviderBuilder
    {
        private string rootPath = string.Empty;
        private IStorageProvider _storageProvider;

        private StorageProviderBuilder(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public static StorageProviderBuilder Create()
        {
            return Create($"{Guid.NewGuid()}.json");
        }
        public static StorageProviderBuilder Create(string rootPath)
        {
            return new(new StorageProvider(rootPath, null));
        }

        public StorageProviderBuilder WithLeague(string guildId, string leagueName)
        {
            var model = new LeagueModel
            {
                Guild = guildId,
                Name = leagueName,
                Description = string.Empty,
                CreatedDate = System.DateTime.UtcNow,
                Status = LeagueStatus.NotSet
            };

            _storageProvider.Add(guildId, StorageProviderConstants.LEAGUE); // ToDo improve this
            var leagues = _storageProvider.Get<LeagueModel>(guildId, StorageProviderConstants.LEAGUE);
            leagues.TryAdd(model.Name, model);

            return this;
        }

        public IStorageProvider Build()
        {
            return _storageProvider;
        }
    }
}
