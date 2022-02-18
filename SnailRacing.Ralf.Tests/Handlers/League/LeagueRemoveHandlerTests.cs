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
                GuildId = "12",
                LeagueName = "League1"
            };
            var storage = new StorageProvider<LeagueStorageProviderModel>();
            var league = new LeagueModel("1", request.LeagueName, string.Empty, DateTime.UtcNow, "", false);

            storage.Store[request.LeagueKey] = league;

            var handler = new LeagueRemoveHandler(storage);

            // act
            var actual = await handler.Handle(request, CancellationToken.None);

            // assert
            Assert.False(actual.HasErrors());
            Assert.Empty(storage.Store);
        }
    }
}
