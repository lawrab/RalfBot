using MediatR;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueJoinHandler : IRequestHandler<LeagueJoinRequest, LeagueJoinResponse>
    {
        private readonly IStorageProvider<LeagueStorageProviderModel> _storage;

        public LeagueJoinHandler(IStorageProvider<LeagueStorageProviderModel> storage)
        {
            _storage = storage;
        }

        public Task<LeagueJoinResponse> Handle(LeagueJoinRequest request, CancellationToken cancellationToken)
        {
            var response = new LeagueJoinResponse();

            _storage.Store[request.LeagueKey]?.Join(request.DiscordMemberId, 
                request.IRacingCustomerId, 
                request.IRacingName, 
                request.AgreeTermsAndConditions);

            return Task.FromResult(response);
        }
    }
}
