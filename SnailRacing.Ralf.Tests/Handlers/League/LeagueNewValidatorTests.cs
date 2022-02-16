﻿using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
using System;
using Xunit;

namespace SnailRacing.Ralf.Tests.Handlers.League
{
    public class LeagueNewValidatorTests
    {
        [Fact]
        public void Empty_LeagueName_Returns_Error()
        {
            // arrange
            var request = new LeagueNewRequest
            {
                LeagueName = String.Empty
            };

            var storage = new StorageProvider<LeagueStorageProviderModel>();
            var validator = new LeagueNewRequestValidator(storage);

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("must not be empty"));

        }

        [Fact]
        public void LeagueName_Exists_Returns_Error()
        {
            // arrange
            var request = new LeagueNewRequest
            {
                LeagueName = "I exist"
            };

            var storage = new StorageProvider<LeagueStorageProviderModel>();
            var league = new LeagueModel(request.LeagueName, string.Empty, DateTime.UtcNow, "");

            storage.Store[request.LeagueName] = league;

            var validator = new LeagueNewRequestValidator(storage);

            // act
            var actual = validator.Validate(request);

            // assert
            Assert.Contains(actual.Errors, (e) => e.ErrorMessage.Contains("already exist"));

        }
    }
}
