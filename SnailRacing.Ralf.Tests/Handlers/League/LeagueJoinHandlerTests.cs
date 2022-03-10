using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
using SnailRacing.Ralf.Tests.Builder;
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
                GuildId = "1",
                LeagueName = "League1",
                DiscordMemberId = "123",
                AgreeTermsAndConditions = true,
                IRacingCustomerId = 12345,
                IRacingName = "Larry Rabbets"

            };
            var leagueStore = StorageProviderBuilder.Create()
                .WithLeague("1", request.LeagueName)
                .Build();

            var handler = new LeagueJoinHandler(leagueStore);

            // act
            var actual = await handler.Handle(request, CancellationToken.None);

            // assert
            var leagues = leagueStore.Get<LeagueModel>(StorageProviderConstants.ROLES);
            var league = leagues[request.LeagueKey];
            var storedParticipant = league.Participants[request.DiscordMemberId];
            Assert.False(actual.HasErrors());
            Assert.NotNull(storedParticipant);
            Assert.Equal(request.DiscordMemberId, storedParticipant?.DiscordMemberId);
            Assert.Equal(request.IRacingCustomerId, storedParticipant?.IRacingCustomerId);
            Assert.Equal(request.IRacingName, storedParticipant?.IRacingName);
            Assert.Equal(request.AgreeTermsAndConditions, storedParticipant?.AgreeTermsAndConditions);
            Assert.Equal(LeagueParticipantStatus.Pending, storedParticipant?.Status);
        }
    }
}
