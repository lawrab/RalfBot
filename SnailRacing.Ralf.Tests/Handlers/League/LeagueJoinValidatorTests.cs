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
        public void Empty_GuildId_Returns_Error()
        {
            // arrange
            var request = new LeagueJoinRequest
            {
                DiscordMemberId = "1",
                LeagueName = "League1",
            };
            var validator = new LeagueJoinRequestValidator(new StorageProvider<LeagueStorageProviderModel>());

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("'Guild Id' must not be empty."));

        }

        [Fact]
        public void Empty_DiscordMemberId_Returns_Error()
        {
            // arrange
            var request = new LeagueJoinRequest
            {
                GuildId = "1",
                LeagueName = "League1",
            };
            var validator = new LeagueJoinRequestValidator(new StorageProvider<LeagueStorageProviderModel>());

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("'Discord Member Id' must not be empty."));

        }

        [Fact]
        public void Invalid_LeagueName_Returns_Error()
        {
            // arrange
            var request = new LeagueJoinRequest
            {
                GuildId = "1",
                DiscordMemberId = "1",
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
                GuildId = "1",
                LeagueName = "League1",
                DiscordMemberId = "123"
            };
            var storage = new StorageProvider<LeagueStorageProviderModel>();
            var league = new LeagueModel("1", request.LeagueName, string.Empty, DateTime.UtcNow, "");
            league.Store.InternalStore![request.DiscordMemberId] = new LeagueParticipantModel();

            storage.Store[request.LeagueKey] = league;

            var validator = new LeagueJoinRequestValidator(storage);

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("You are already a"));

        }
    }
}
