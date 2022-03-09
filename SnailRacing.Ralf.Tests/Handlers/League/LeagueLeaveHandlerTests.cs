using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
using SnailRacing.Ralf.Tests.Builder;
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
            var leagueStore = StorageProviderBuilder.Create()
                 .WithLeague(request.GuildId, request.LeagueName)
                 .Build();

            var league = leagueStore.Get<LeagueModel>(new(request.GuildId, request.LeagueKey));

            league.Join(request.DiscordMemberId, 0, string.Empty, true, LeagueParticipantStatus.Approved);

            //storage.Store[request.LeagueKey] = league;

            var handler = new LeagueLeaveHandler(leagueStore);

            // act
            var actual = await handler.Handle(request, CancellationToken.None);

            // assert
            var leagueActual = leagueStore.Get<LeagueModel>(new(request.GuildId, request.LeagueKey));
            Assert.False(actual.HasErrors());
            //Assert.False(leagueActual.ContainsKey(request.DiscordMemberId));
            Assert.False(true);
        }
    }
}
