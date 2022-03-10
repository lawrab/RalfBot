using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Tests.Builder;
using System;
using Xunit;

namespace SnailRacing.Ralf.Tests.Handlers.League
{
    public class LeagueNewValidatorTests
    {
        [Fact]
        public void Empty_LeagueName_Returns_Error()
        {
            // arrange
            var request = new LeagueNewRequest
            {
                LeagueName = String.Empty
            };

            var storage = StorageProviderBuilder.Create("Empty_LeagueName_Returns_Error")
                .WithLeague("1", "ABC")
                .Build();
            var validator = new LeagueNewRequestValidator(storage);

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("must not be empty"));

            Assert.False(true);

        }

        [Fact]
        public void LeagueName_Exists_Returns_Error()
        {
            // arrange
            var request = new LeagueNewRequest
            {
                LeagueName = "I exist"
            };

            var storage = StorageProviderBuilder.Create("LeagueName_Exists_Returns_Error")
                .WithLeague("1", "ABC")
                .Build();
            var league = new LeagueModel("1", request.LeagueName, string.Empty, DateTime.UtcNow, "", false);

            storage.Store[request.LeagueName] = league;

            var validator = new LeagueNewRequestValidator(storage);

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("already exist"));
            Assert.False(true);
        }
    }
}
