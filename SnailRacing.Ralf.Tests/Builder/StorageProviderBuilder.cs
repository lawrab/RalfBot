using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
using SnailRacing.Store;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SnailRacing.Ralf.Tests.Builder
{
    // ToDo: rewrite and implement it properly
    internal class StorageProviderBuilder
    {
        private string rootPath = string.Empty;
        private IStorageProvider _storageProvider;

        private StorageProviderBuilder(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public static StorageProviderBuilder Create(bool reset = false)
        {
            return Create($"{Guid.NewGuid()}.json", reset);
        }
        public static StorageProviderBuilder Create(string rootPath, bool reset = false)
        {
            if(reset)
            {
                RemoveAllFiles(rootPath);
            }
            return new(new StorageProvider(rootPath, null));
        }

        private static void RemoveAllFiles(string rootPath)
        {
            if (!Directory.Exists(rootPath)) return;
            Directory.Delete(rootPath, true);
        }

        public StorageProviderBuilder WithLeague(string guildId, string leagueName, IEnumerable<LeagueParticipantModel>? participants = null)
        {
            var model = new LeagueModel
            {
                Guild = guildId,
                Name = leagueName,
                Description = string.Empty,
                CreatedDate = DateTime.UtcNow,
                Status = LeagueStatus.NotSet
            };

            if (participants != null)
            {
                model.Participants = new ConcurrentDictionary<string, LeagueParticipantModel>(participants.ToDictionary((m) => m.DiscordMemberId));
            }

            _storageProvider.Add(guildId, StorageProviderConstants.LEAGUE); // ToDo improve this
            var leagues = _storageProvider.Get<LeagueModel>(guildId, StorageProviderConstants.LEAGUE);
            leagues.TryAdd(model.Name.ToUpper(), model);

            return this;
        }

        public IStorageProvider Build()
        {
            return _storageProvider;
        }
    }
}
