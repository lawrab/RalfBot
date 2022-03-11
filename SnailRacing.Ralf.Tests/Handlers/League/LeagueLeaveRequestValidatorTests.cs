using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Tests.Builder;
using Xunit;

namespace SnailRacing.Ralf.Tests.Handlers.League
{
    public class LeagueLeaveRequestValidatorTests
    {
        [Fact]
        public void Empty_GuildId_Returns_Error()
        {
            // arrange
            var request = new LeagueLeaveRequest
            {
                DiscordMemberId = "1",
                LeagueName = "League1",
            };

            var storage = StorageProviderBuilder.Create()
                .WithLeague("1", request.LeagueName)
                .Build();

            var validator = new LeagueLeaveRequestValidator(storage);

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.False(actual.IsValid);
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("'Guild Id' must not be empty."));
        }

        [Fact]
        public void Empty_DiscordMemberId_Returns_Error()
        {
            // arrange
            var request = new LeagueLeaveRequest
            {
                GuildId = "1",
                LeagueName = "League1",
            };

            var storage = StorageProviderBuilder.Create()
                .WithLeague("1", request.LeagueName)
                .Build();
            var validator = new LeagueLeaveRequestValidator(storage);

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("'Discord Member Id' must not be empty."));
        }

        [Fact]
        public void Invalid_LeagueName_Returns_Error()
        {
            // arrange
            var request = new LeagueLeaveRequest
            {
                GuildId = "1",
                DiscordMemberId = "1",
                LeagueName = "Unknown"
            };
            var storage = StorageProviderBuilder.Create()
                .WithLeague("1", "Known")
                .Build();
            var validator = new LeagueLeaveRequestValidator(storage);

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("do not exist"));
        }

        [Fact]
        public void A_Member_Of_League_Returns_No_Error()
        {
            // arrange
            var request = new LeagueLeaveRequest
            {
                GuildId = "1",
                LeagueName = "League1",
                DiscordMemberId = "123"
            };

            var storage = StorageProviderBuilder.Create("A_Member_Of_League_Returns_No_Error")
                .WithLeague(request.GuildId, request.LeagueName, new[] { new LeagueParticipantModel { DiscordMemberId = request.DiscordMemberId } })
                .Build();
            var validator = new LeagueLeaveRequestValidator(storage);

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.True(actual.IsValid);
        }

        [Fact]
        public void Not_A_Member_Of_League_Returns_Error()
        {
            // arrange
            var request = new LeagueLeaveRequest
            {
                GuildId = "1",
                LeagueName = "League1",
                DiscordMemberId = "123"
            };

            var storage = StorageProviderBuilder.Create("Not_A_Member_Of_League_Returns_Error", true)
                .WithLeague(request.GuildId, request.LeagueName, new[] { new LeagueParticipantModel { DiscordMemberId = "234" } })
                .Build();
            var validator = new LeagueLeaveRequestValidator(storage);

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage == string.Format(Messages.NOT_MEMBER_OF_LEAGUE, request.LeagueName));
        }
    }
}
