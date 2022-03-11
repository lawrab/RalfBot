using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
using SnailRacing.Ralf.Tests.Builder;
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
            // arrange
            var request = new LeagueQueryRequest
            {
                GuildId = "1"
            };
            var storage = StorageProviderBuilder.Create("Query_League_Without_Predicate_Returns_All_Leagues_For_Guild", true)
                .WithLeague("1", "1")
                .WithLeague("1", "2")
                .WithLeague("2", "1")
                .Build();

            var handler = new LeagueQueryHandler(storage);

            // act
            var actual = await handler.Handle(request, CancellationToken.None);

            // assert
            Assert.False(actual.HasErrors());
            Assert.Equal(2, actual.Leagues.Count());
        }
    }
}
