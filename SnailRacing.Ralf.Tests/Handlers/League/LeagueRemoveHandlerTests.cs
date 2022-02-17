using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SnailRacing.Ralf.Tests.Handlers.League
{
    public class LeagueRemoveHandlerTests
    {
        [Fact]
        public async Task Joins_League_With_Valid_League_Request()
        {
            // arrange
            var request = new LeagueRemoveRequest
            {
                LeagueName = "League1"
            };
            var storage = new StorageProvider<LeagueStorageProviderModel>();
            var league = new LeagueModel(request.LeagueName, string.Empty, DateTime.UtcNow, "");

            storage.Store[request.LeagueName] = league;

            var handler = new LeagueRemoveHandler(storage);

            // act
            var actual = await handler.Handle(request, CancellationToken.None);

            // assert
            Assert.False(actual.HasErrors());
            Assert.False(league.Store.InternalStore!.ContainsKey(request.LeagueName));
        }
    }
}
