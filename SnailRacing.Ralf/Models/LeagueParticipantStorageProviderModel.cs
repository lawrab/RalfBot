using DSharpPlus.Entities;
using System.Collections;
using System.Collections.Concurrent;

namespace SnailRacing.Ralf.Models
{
    public class LeagueParticipantStorageProviderModel : StorageProviderModelBase<ConcurrentDictionary<string, LeagueParticipantModel>>, 
        IEnumerable<KeyValuePair<string, LeagueParticipantModel>>
    {
        public LeagueParticipantStorageProviderModel()
        {
            _data = new ConcurrentDictionary<string, LeagueParticipantModel>();
        }

        public LeagueParticipantModel? this[string key]
        {
            get
            {
                return _data?[key];
            }
            set
            {
                if (_data is null) throw new ArgumentException("_data property not initialised");
                _data[key] = value!;
                _saveData();
            }
        }

        public bool IsMember(string discordMemeberId)
        {
            return InternalStore?.ContainsKey(discordMemeberId) == true;
        }
        public void JoinLeague(string discordMemberId, int clientId, string fullName, bool agreeTermsAndConditions)
        {
            if (InternalStore?.ContainsKey(discordMemberId) == true) return;

            this[discordMemberId] = new LeagueParticipantModel
            {
                DiscordMemberId = discordMemberId,
                IRacingCustomerId = clientId,
                IRacingName = fullName,
                RegistrationDate = DateTime.UtcNow,
                Status = LeagueParticipantStatus.Pending,
                AgreeTermsAndConditions = agreeTermsAndConditions
            };
        }

        public void LeaveLeague(string discordMemberId)
        {
            if (InternalStore?.ContainsKey(discordMemberId) == false) return;
            InternalStore?.Remove(discordMemberId, out _);
        }

        public IEnumerator<KeyValuePair<string, LeagueParticipantModel>> GetEnumerator()
        {
            if (_data is null) return Enumerable.Empty<KeyValuePair<string, LeagueParticipantModel>>().GetEnumerator();

            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_data is null) return Enumerable.Empty<KeyValuePair<string, LeagueParticipantModel>>().GetEnumerator();

            return _data.GetEnumerator();
        }
    }
}
