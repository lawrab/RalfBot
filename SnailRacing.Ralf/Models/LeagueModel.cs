using DSharpPlus.Entities;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Models
{
    public class LeagueModel : StorageProvider<LeagueParticipantStorageProviderModel>
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public LeagueStatus Status { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public string StoragePath { get; }
        public Uri? Standings { get; private set; } = new Uri("https://annieandlarry.com");
        
        public LeagueModel(string name, string description, DateTime createdDate, string storagePath)
        {
            Name = name;
            Description = description;
            CreatedDate = createdDate;
            StoragePath = storagePath;
            InitStorage();
        }


        public void Join(DiscordMember member, int clientId, string fullName)
        {
            Store.JoinLeague(member, clientId, fullName);
        }

        private void InitStorage()
        {
            SetFileStorageProvider(new JsonFileStorageProvider(Path.Combine(StoragePath, $"{Name}LeagueStorage.json")))
                .ConfigureAwait(false);
        }
    }
}
