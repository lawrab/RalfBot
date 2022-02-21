using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SnailRacing.Ralf.Tests.Handlers.League
{
    public class LeagueLeaveHandlerTests
    {
        [Fact]
        public async Task Leaves_League_With_Valid_League_Request_Removes_Participant()
        {
            // arrange
            var request = new LeagueLeaveRequest
            {
                GuildId = "1",
                LeagueName = "League1",
                DiscordMemberId = "123"
            };
            var storage = new StorageProvider<LeagueStorageProviderModel>();
            var league = new LeagueModel("1", request.LeagueName, string.Empty, DateTime.UtcNow, "", false);

            league.Store.JoinLeague(request.DiscordMemberId, 0, string.Empty, true);

            storage.Store[request.LeagueKey] = league;

            var handler = new LeagueLeaveHandler(storage);

            // act
            var actual = await handler.Handle(request, CancellationToken.None);

            // assert
            Assert.False(actual.HasErrors());
            Assert.False(league.Store.InternalStore!.ContainsKey(request.DiscordMemberId));
        }
    }
}
