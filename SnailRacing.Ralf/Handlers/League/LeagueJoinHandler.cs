using MediatR;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueJoinHandler : IRequestHandler<LeagueJoinRequest, LeagueJoinResponse>
    {
        private readonly IStorageProvider _storageProvider;

        public LeagueJoinHandler(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public Task<LeagueJoinResponse> Handle(LeagueJoinRequest request, CancellationToken cancellationToken)
        {
            var response = new LeagueJoinResponse();
            var store = StoreHelper.GetLeagueStore(request.GuildId, _storageProvider);
            var league = store[request.LeagueKey];

            var approvedMembers = league.Participants.Count(p => p.Value.Status == LeagueParticipantStatus.Approved);
            var status = league.Status == LeagueStatus.Open && approvedMembers < league.MaxGrid ?
                LeagueParticipantStatus.Approved : LeagueParticipantStatus.Pending;

            league.Join(request.DiscordMemberId,
                request.DiscordMemberUser,
                request.IRacingCustomerId,
                request.IRacingName,
                request.AgreeTermsAndConditions,
                status);

            if (league.MaxGrid.HasValue &&
                league.Status == LeagueStatus.Open
                && approvedMembers >= league.MaxGrid)
            {
                league.Status = LeagueStatus.Closed;
                response.MaxApprovedReached = true;
            }

            store.TryUpdate(request.LeagueKey, league);
            return Task.FromResult(response);
        }
    }
}
