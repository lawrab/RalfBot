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
        public async Task Query_League_Without_Predicate_Returns_All_Leagues()
        {
            // arrange
            var request = new LeagueQueryRequest();
            var storage = new StorageProvider<LeagueStorageProviderModel>();

            var expected = new List<LeagueModel>
            {
                new LeagueModel("1", string.Empty, DateTime.UtcNow, ""),
                new LeagueModel("2", string.Empty, DateTime.UtcNow, "")
            };

            storage.Store["1"] = expected[0];
            storage.Store["2"] = expected[1];

            var handler = new LeagueQueryHandler(storage);

            // act
            var actual = await handler.Handle(request, CancellationToken.None);

            // assert
            Assert.False(actual.HasErrors());
            Assert.Equal(expected.Select(x => x.Name), actual.Leagues.Select(x => x.Name));
        }
    }
}
