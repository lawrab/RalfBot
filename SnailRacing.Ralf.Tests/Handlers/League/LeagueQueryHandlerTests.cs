using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SnailRacing.Ralf.Tests.Handlers.League
{
    public class LeagueQueryHandlerTests
    {
        [Fact]
        public async Task Query_League_Without_Predicate_Returns_All_Leagues_For_Guild()
        {
            ////// arrange
            ////var request = new LeagueQueryRequest
            ////{
            ////    GuildId = "1"
            ////};
            ////var storage = new StorageProvider<LeagueStorageProviderModel>();

            ////var expected = new List<LeagueModel>
            ////{
            ////    new LeagueModel("1", "1", string.Empty, DateTime.UtcNow, "", false),
            ////    new LeagueModel("1", "2", string.Empty, DateTime.UtcNow, "", false)
            ////};

            ////storage.Store["1"] = expected[0];
            ////storage.Store["2"] = expected[1];
            ////storage.Store["3"] = new LeagueModel("2", "3", string.Empty, DateTime.UtcNow, "", false);

            ////var handler = new LeagueQueryHandler(storage);

            ////// act
            ////var actual = await handler.Handle(request, CancellationToken.None);

            ////// assert
            ////Assert.False(actual.HasErrors());
            ////Assert.Equal(expected.Select(x => x.Name).OrderBy(x => x), actual.Leagues.Select(x => x.Name).OrderBy(x => x));
            ///
            Assert.False(true);

        }
    }
}
