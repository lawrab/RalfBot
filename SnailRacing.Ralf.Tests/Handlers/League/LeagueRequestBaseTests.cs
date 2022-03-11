using SnailRacing.Ralf.Handlers.League;
using Xunit;

namespace SnailRacing.Ralf.Tests.Handlers.League
{
    public class LeagueRequestBaseTests
    {
        [Fact]
        public void LegueKey_Returns_Correct_Value()
        {
            // arrange
            var request = new LeagueRequestBase
            {
                GuildId = "Guild1",
                LeagueName = "RalfLeague"
            };

            // act
            var actual = request.LeagueKey;

            // assert
            Assert.Equal("RalfLeague".ToUpper(), actual);
        }
    }
}
