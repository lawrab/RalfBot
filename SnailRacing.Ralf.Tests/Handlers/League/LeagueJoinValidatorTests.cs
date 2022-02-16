using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
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
    }
}
