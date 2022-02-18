using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Models
{
    public class LeagueModel : StorageProvider<LeagueParticipantStorageProviderModel>
    {
        public string Guild { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public LeagueStatus Status { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public string StoragePath { get; }
        public Uri? Standings { get; private set; } = new Uri("https://annieandlarry.com");
        
        public LeagueModel(string guild, string name, string description, DateTime createdDate, string storagePath)
        {
            Guild = guild;
            Name = name;
            Description = description;
            CreatedDate = createdDate;
            StoragePath = storagePath;
            InitStorage();
        }

        public bool Join(string discordMemberId, int clientId, string fullName)
        {
            return Store.JoinLeague(discordMemberId, clientId, fullName);
        }

        private void InitStorage()
        {
            SetFileStorageProvider(new JsonFileStorageProvider(Path.Combine(StoragePath, $"{Guild}-{Name}-LeagueStorage.json")))
                .ConfigureAwait(false);
        }
    }
}
