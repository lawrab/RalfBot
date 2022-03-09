using System.Collections.Concurrent;

namespace SnailRacing.Ralf.Models
{
    public class LeagueModel
    {
        public string Guild { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public LeagueStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public Uri? Standings { get; set; } = new Uri("https://annieandlarry.com");
        public int? MaxGrid { get; set; }
        public ConcurrentDictionary<string, LeagueParticipantModel> Participants { get; set; } = new();

        public void Join(string discordMemberId, int clientId, string fullName, bool agreeTermsAndConditions, LeagueParticipantStatus status)
        {
            Participants.TryAdd(discordMemberId, new LeagueParticipantModel
            {
                DiscordMemberId = discordMemberId,
                IRacingCustomerId = clientId,
                IRacingName = fullName,
                AgreeTermsAndConditions = agreeTermsAndConditions,
                RegistrationDate = DateTime.UtcNow,
                Status = status
            });
        }

        ////public void Leave(string discordMemberId)
        ////{
        ////    Store.LeaveLeague(discordMemberId);
        ////}

        ////public void Approve(string discordMemberId, string approvedBy)
        ////{
        ////    Store.ApproveParticipant(discordMemberId, approvedBy);
        ////}

        ////private void InitStorage()
        ////{
        ////    SetFileStorageProvider(new JsonFileStorageProvider(Path.Combine(StoragePath, $"{Guild}-{Name}-LeagueStorage.json")))
        ////        .ConfigureAwait(false);
        ////}
    }
}
