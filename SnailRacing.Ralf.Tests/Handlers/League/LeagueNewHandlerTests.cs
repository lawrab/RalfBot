using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
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
                LeagueName = "League1",
                Description = "Desc1"
            };
            var storage = new StorageProvider<LeagueStorageProviderModel>();

            var handler = new LeagueNewHandler(storage, new AppConfig());

            // act
            var actual = await handler.Handle(request, CancellationToken.None);

            // assert
            var storedLeagueActual = storage.Store[request.LeagueName] ?? new LeagueModel(string.Empty, string.Empty, DateTime.MinValue, string.Empty);
            Assert.False(actual.HasErrors());
            Assert.True(storage.Store.InternalStore!.ContainsKey(request.LeagueName));
            Assert.Equal(request.LeagueName, storedLeagueActual.Name);
            Assert.Equal(request.Description, storedLeagueActual.Description);
        }
    }
}
