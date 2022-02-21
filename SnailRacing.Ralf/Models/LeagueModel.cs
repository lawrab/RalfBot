using SnailRacing.Ralf.Providers;
using System.Text.Json.Serialization;

namespace SnailRacing.Ralf.Models
{
    public class LeagueModel : StorageProvider<LeagueParticipantStorageProviderModel>
    {
        public string Guild { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public LeagueStatus Status { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public string StoragePath { get; private set; }
        public Uri? Standings { get; private set; } = new Uri("https://annieandlarry.com");

        [JsonConstructor]
        public LeagueModel(string guild, string name, string description, DateTime createdDate, string storagePath)
            : this(guild, name, description, createdDate, storagePath, true)
        { 
        }

        public LeagueModel(string guild, string name, string description, DateTime createdDate, string storagePath, bool initStorage)
        {
            Guild = guild;
            Name = name;
            Description = description;
            CreatedDate = createdDate;
            StoragePath = storagePath;
            if (initStorage)
            {
                InitStorage();
            }
        }

        public void Join(string discordMemberId, int clientId, string fullName, bool agreeTermsAndConditions)
        {
            Store.JoinLeague(discordMemberId, clientId, fullName, agreeTermsAndConditions);
        }

        public void Leave(string discordMemberId)
        {
            Store.LeaveLeague(discordMemberId);
        }

        private void InitStorage()
        {
            SetFileStorageProvider(new JsonFileStorageProvider(Path.Combine(StoragePath, $"{Guild}-{Name}-LeagueStorage.json")))
                .ConfigureAwait(false);
        }
    }
}
