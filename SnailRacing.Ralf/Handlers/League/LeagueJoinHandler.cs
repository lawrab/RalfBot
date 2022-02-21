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

            var approvedMembers = league!.Store.Count(p => p.Value.Status == LeagueParticipantStatus.Approved);
            var status = league.Status == LeagueStatus.Open && approvedMembers < league.MaxGrid ?
                LeagueParticipantStatus.Approved : LeagueParticipantStatus.Pending;

            league!.Join(request.DiscordMemberId, 
                request.IRacingCustomerId, 
                request.IRacingName, 
                request.AgreeTermsAndConditions,
                status);

            if(league.MaxGrid.HasValue && 
                league.Status == LeagueStatus.Open
                && approvedMembers >= league.MaxGrid)
            {
                _storage.Store.SetClosed(request.LeagueKey);
                response.MaxApprovedReached = true;
            }

            return Task.FromResult(response);
        }
    }
}
