﻿using FluentValidation;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueJoinHandler : ICommand<LeagueJoinRequest, LeagueJoinResponse>
    {
        private readonly IStorageProvider<LeagueStorageProviderModel> _storage;
        private readonly IValidator<LeagueJoinRequest> _validator;

        public LeagueJoinHandler(IStorageProvider<LeagueStorageProviderModel> storage,
            IValidator<LeagueJoinRequest> validator)
        {
            _storage = storage;
            _validator = validator;
        }
        public Task<LeagueJoinResponse> Handle(LeagueJoinRequest request)
        {
            var validationResponse = _validator.Validate(request);

            var response = new LeagueJoinResponse
            {
                Errors = validationResponse.Errors.Select(e => e.ErrorMessage)
            };

            _storage.Store[request.LeagueName]?.Join(request.DiscordMemberId, 0, string.Empty);

            return Task.FromResult(response);
        }
    }
}
