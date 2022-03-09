using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Tests.Builder;
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

            var leagueStore = StorageProviderBuilder.Create()
                .WithLeague("1", "ABC")
                .Build();
            var validator = new LeagueRemoveRequestValidator(leagueStore);

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("must not be empty"));
        }
    }
}
