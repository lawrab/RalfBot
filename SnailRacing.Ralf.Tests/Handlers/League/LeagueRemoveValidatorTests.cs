using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
using Xunit;

namespace SnailRacing.Ralf.Tests.Handlers.League
{
    public class LeagueRemoveValidatorTests
    {
        [Fact]
        public void Invalid_LeagueName_Returns_Error()
        {
            // arrange
            var request = new LeagueRemoveRequest
            {
                LeagueName = string.Empty
            };

            var storage = new StorageProvider<LeagueStorageProviderModel>();
            var validator = new LeagueRemoveRequestValidator(storage);

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("must not be empty"));
        }
    }
}
