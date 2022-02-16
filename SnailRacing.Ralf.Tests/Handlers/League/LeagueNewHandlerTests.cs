using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
using System;
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
            var validator = new LeagueNewRequestValidator(storage);

            var handler = new LeagueNewHandler(storage, validator, new AppConfig());

            // act
            var actual = await handler.Handle(request);

            // assert
            var storedLeagueActual = storage.Store[request.LeagueName] ?? new LeagueModel(string.Empty, string.Empty, DateTime.MinValue, string.Empty);
            Assert.False(actual.HasErrors());
            Assert.True(storage.Store.InternalStore!.ContainsKey(request.LeagueName));
            Assert.Equal(request.LeagueName, storedLeagueActual.Name);
            Assert.Equal(request.Description, storedLeagueActual.Description);
        }

        [Fact]
        public async Task LeagueNewRequest_With_Errors_Dont_Add_League()
        {
            // arrange
            var request = new LeagueNewRequest
            {
                LeagueName = "League1",
                Description = "Desc1"
            };
            var storage = new StorageProvider<LeagueStorageProviderModel>();
            var league = new LeagueModel(request.LeagueName, string.Empty, DateTime.UtcNow, "");

            storage.Store[request.LeagueName] = league;

            var validator = new LeagueNewRequestValidator(storage);
            var handler = new LeagueNewHandler(storage, validator, new AppConfig());

            // act
            var actual = await handler.Handle(request);

            // assert
            Assert.True(actual.HasErrors());
        }

    }
}
