using MediatR;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueParticipantApprovalHandler : IRequestHandler<LeagueParticipantApproalRequest, LeagueParticipantApprovalResponse>
    {
        private readonly IStorageProvider _storageProvider;

        public LeagueParticipantApprovalHandler(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public Task<LeagueParticipantApprovalResponse> Handle(LeagueParticipantApproalRequest request, CancellationToken cancellationToken)
        {
            var response = new LeagueParticipantApprovalResponse();

            var store = StoreHelper.GetLeagueStore(request.GuildId, _storageProvider);
            var league = store[request.LeagueKey];

            league.Participants[request.DiscordMemberId].Status = LeagueParticipantStatus.Approved;
            league.Participants[request.DiscordMemberId].ApprovedBy = request.ApprovedBy;

            store.TryUpdate(request.LeagueKey, league);

            return Task.FromResult(response);
        }
    }
}
