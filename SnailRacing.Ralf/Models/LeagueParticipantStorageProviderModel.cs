﻿using DSharpPlus.Entities;
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
        public bool JoinLeague(DiscordMember member, int clientId, string fullName)
        {
            var key = member.Id.ToString();
            if (InternalStore?.ContainsKey(key) == true) return false;

            this[key] = new LeagueParticipantModel
            {
                DiscordNickname = member.DisplayName,
                IRacingClientId = clientId,
                IRacingName = fullName,
                RegistrationDate = DateTime.UtcNow,
                Email = member.Email,
                Status = LeagueParticipantStatus.Pending
            };

            return true;
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
