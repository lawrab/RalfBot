using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
using SnailRacing.Ralf.Tests.Builder;
using SnailRacing.Store;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SnailRacing.Ralf.Tests.Handlers.League
{
    public class LeagueNewHandlerTests
    {

        [Fact]
        public async Task LeagueNewRequest_Adds_New_League()
        {
            // arrange
            var request = new LeagueNewRequest
            {
                GuildId = "1",
                LeagueName = "League1",
                Description = "Desc1"
            };
            var storage = StorageProviderBuilder.Create("LeagueNewRequest_Adds_New_League")
                .WithLeague("1", "ABC")
                .Build();

            var handler = new LeagueNewHandler(storage, new AppConfig());

            // act
            var actual = await handler.Handle(request, CancellationToken.None);

            // assert
            var league = StoreHelper.GetLeague(request.GuildId, request.LeagueKey, storage);
            Assert.False(actual.HasErrors());
            Assert.NotNull(league);
            Assert.Equal(request.LeagueName, league.Name);
            Assert.Equal(request.Description, league.Description);
        }
    }
}
