using MediatR;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueParticipantApprovalHandler : IRequestHandler<LeagueParticipantApproalRequest, LeagueParticipantApprovalResponse>
    {
        private readonly IStorageProvider<LeagueStorageProviderModel> _storage;

        public LeagueParticipantApprovalHandler(IStorageProvider<LeagueStorageProviderModel> storage)
        {
            _storage = storage;
        }

        public Task<LeagueParticipantApprovalResponse> Handle(LeagueParticipantApproalRequest request, CancellationToken cancellationToken)
        {
            var response = new LeagueParticipantApprovalResponse();
            var league = _storage.Store[request.LeagueKey];

            league!.Approve(request.DiscordMemberId, request.ApprovedBy);

            return Task.FromResult(response);
        }
    }
}
