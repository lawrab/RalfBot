using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Infrastrtucture;
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
            var storage = StorageProviderBuilder.Create()
                 .WithLeague(request.GuildId, request.LeagueName, new[] { new LeagueParticipantModel { DiscordMemberId = request.DiscordMemberId } })
                 .Build();

            var league = StoreHelper.GetLeague(request.GuildId, request.LeagueKey, storage);

            league.Join(request.DiscordMemberId, 0, string.Empty, true, LeagueParticipantStatus.Approved);

            //storage.Store[request.LeagueKey] = league;

            var handler = new LeagueLeaveHandler(storage);

            // act
            var actual = await handler.Handle(request, CancellationToken.None);

            // assert
            var leagueActual = StoreHelper.GetLeague(request.GuildId, request.LeagueKey, storage);
            Assert.False(actual.HasErrors());
            Assert.False(leagueActual.Participants.ContainsKey(request.DiscordMemberId));
        }
    }
}
