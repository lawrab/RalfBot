using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
using SnailRacing.Ralf.Tests.Builder;
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
        public async Task Remove_League_With_Valid_League_Request()
        {
            // arrange
            var request = new LeagueRemoveRequest
            {
                GuildId = "12",
                LeagueName = "League1"
            };
            var storage = StorageProviderBuilder.Create("16Not_A_Member_Of_League_Returns_Error", true)
                .WithLeague(request.GuildId, request.LeagueName, new[] { new LeagueParticipantModel { DiscordMemberId = "234" } })
                .Build();

            var handler = new LeagueRemoveHandler(storage);

            // act
            var actual = await handler.Handle(request, CancellationToken.None);

            // assert
            Assert.Throws<KeyNotFoundException>(() => StoreHelper.GetLeague(request.GuildId, request.LeagueKey, storage));
        }
    }
}
