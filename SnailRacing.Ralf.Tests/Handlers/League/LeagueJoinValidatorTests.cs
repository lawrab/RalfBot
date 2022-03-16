using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
using SnailRacing.Ralf.Tests.Builder;
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

            var storage = StorageProviderBuilder.Create("2Empty_GuildId_Returns_Error", true)
                .Build();
            var validator = new LeagueJoinRequestValidator(storage);

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
            var storage = StorageProviderBuilder.Create("3Empty_DiscordMemberId_Returns_Error", true)
                .Build();
            var validator = new LeagueJoinRequestValidator(storage);

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
                DiscordMemberUser = "ABC",
                LeagueName = "I do not exist"
            };
            var storage = StorageProviderBuilder.Create("4Invalid_LeagueName_Returns_Error", true)
                .WithLeague(request.GuildId, "ABC")
                .Build();
            var validator = new LeagueJoinRequestValidator(storage);

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
                DiscordMemberId = "123",
                DiscordMemberUser = "ABC"
            };

            var storage = StorageProviderBuilder.Create("5Allready_Joined_League_Returns_Error", true)
                .WithLeague(request.GuildId, request.LeagueName, new[] { new LeagueParticipantModel { DiscordMemberId = request.DiscordMemberId, DicordMemberUser = request.DiscordMemberUser} })
                .Build();

            var league = StoreHelper.GetLeague(request.GuildId, request.LeagueKey, storage);

            var validator = new LeagueJoinRequestValidator(storage);

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("You are already a"));
        }
    }
}
