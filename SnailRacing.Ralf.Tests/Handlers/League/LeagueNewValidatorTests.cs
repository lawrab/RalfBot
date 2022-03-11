using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Tests.Builder;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SnailRacing.Ralf.Tests.Handlers.League
{
    public class LeagueNewValidatorTests
    {
        [Fact]
        public void Empty_GuildId_Returns_Error()
        {
            // arrange
            var request = new LeagueNewRequest();

            var validator = new LeagueNewRequestValidator(null);

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.False(actual.IsValid);
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("must not be empty"));
        }

        [Fact]
        public void Empty_LeagueName_Returns_Error()
        {
            // arrange
            var request = new LeagueNewRequest
            {
                GuildId = "1",
                LeagueName = String.Empty
            };

            var storage = StorageProviderBuilder.Create("13Empty_LeagueName_Returns_Error", true)
                .WithLeague("1", "ABC")
                .Build();
            var validator = new LeagueNewRequestValidator(storage);

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.False(actual.IsValid);
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("must not be empty"));
        }

        [Fact]
        public async Task LeagueName_Exists_Returns_Error()
        {
            // arrange
            var request = new LeagueNewRequest
            {
                GuildId = "1",
                LeagueName = "ABC"
            };

            var storage = StorageProviderBuilder.Create("14LeagueName_Exists_Returns_Error", true)
                .WithLeague(request.GuildId, request.LeagueName)
                .Build();

            var validator = new LeagueNewRequestValidator(storage);

            // act
            var actual = await validator.ValidateAsync(request);

            // assert
            Assert.False(actual.IsValid);
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("already exist"));
        }
    }
}
