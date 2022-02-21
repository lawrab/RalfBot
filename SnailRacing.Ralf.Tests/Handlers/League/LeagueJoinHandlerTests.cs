﻿using SnailRacing.Ralf.Handlers.League;
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
                GuildId = "1",
                LeagueName = "League1",
                DiscordMemberId = "123",
                AgreeTermsAndConditions = true,
                IRacingCustomerId = 12345,
                IRacingName = "Larry Rabbets"

            };
            var storage = new StorageProvider<LeagueStorageProviderModel>();
            var league = new LeagueModel("1", request.LeagueName, string.Empty, DateTime.UtcNow, "", false);

            storage.Store[request.LeagueKey] = league;

            var handler = new LeagueJoinHandler(storage);

            // act
            var actual = await handler.Handle(request, CancellationToken.None);

            // assert
            var storedParticipant = league.Store.InternalStore?[request.DiscordMemberId];
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
