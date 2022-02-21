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
            var league = _storage.Store[request.LeagueKey];

            league!.Join(request.DiscordMemberId, 
                request.IRacingCustomerId, 
                request.IRacingName, 
                request.AgreeTermsAndConditions);

            var approvedMembers = league!.Store.Count(p => p.Value.Status == LeagueParticipantStatus.Approved);

            if(league.MaxGrid.HasValue && 
                league.Status == LeagueStatus.Open
                && approvedMembers >= league.MaxGrid)
            {
                _storage.Store.SetClosed(request.LeagueKey);
            }

            return Task.FromResult(response);
        }
    }
}
