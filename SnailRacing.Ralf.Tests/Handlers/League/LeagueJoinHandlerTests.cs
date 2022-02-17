using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SnailRacing.Ralf.Tests.Handlers.League
{
    public class LeagueJoinHandlerTests
    {
        [Fact]
        public async Task Joins_League_With_Valid_League_Request()
        {
            // arrange
            var request = new LeagueJoinRequest
            {
                LeagueName = "League1",
                DiscordMemberId = "123"
            };
            var storage = new StorageProvider<LeagueStorageProviderModel>();
            var league = new LeagueModel(request.LeagueName, string.Empty, DateTime.UtcNow, "");

            storage.Store[request.LeagueName] = league;

            var validator = new LeagueJoinRequestValidator(storage);
            var handler = new LeagueJoinHandler(storage, validator);

            // act
            var actual = await handler.Handle(request, CancellationToken.None);

            // assert
            Assert.False(actual.HasErrors());
            Assert.True(league.Store.InternalStore!.ContainsKey(request.DiscordMemberId));
        }
    }
}
