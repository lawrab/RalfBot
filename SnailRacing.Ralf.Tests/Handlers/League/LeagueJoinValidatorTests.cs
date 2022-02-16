using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
using System;
using Xunit;

namespace SnailRacing.Ralf.Tests.Handlers.League
{
    public class LeagueJoinValidatorTests
    {
        [Fact]
        public void Invalid_LeagueName_Returns_Error()
        {
            // arrange
            var request = new LeagueJoinRequest
            {
                LeagueName = "I do not exist"
            };
            var validator = new LeagueJoinRequestValidator(new StorageProvider<LeagueStorageProviderModel>());

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("do not exist"));

        }

        [Fact]
        public void Allready_Joined_League_Returns_Error()
        {
            // arrange
            var request = new LeagueJoinRequest
            {
                LeagueName = "League1",
                DiscordMemberId = "123"
            };
            var storage = new StorageProvider<LeagueStorageProviderModel>();
            var league = new LeagueModel(request.LeagueName, string.Empty, DateTime.UtcNow, "");
            league.Store.InternalStore![request.DiscordMemberId] = new LeagueParticipantModel();

            storage.Store[request.LeagueName] = league;

            var validator = new LeagueJoinRequestValidator(storage);

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("You are already a"));

        }
    }
}
