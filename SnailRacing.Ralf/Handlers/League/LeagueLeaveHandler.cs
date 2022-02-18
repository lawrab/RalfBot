using MediatR;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueLeaveHandler : IRequestHandler<LeagueLeaveRequest, LeagueLeaveResponse>
    {
        private readonly IStorageProvider<LeagueStorageProviderModel> _storage;

        public LeagueLeaveHandler(IStorageProvider<LeagueStorageProviderModel> storage)
        {
            _storage = storage;
        }

        public Task<LeagueLeaveResponse> Handle(LeagueLeaveRequest request, CancellationToken cancellationToken)
        {
            var response = new LeagueLeaveResponse();

            _storage.Store[request.LeagueKey]?.Leave(request.DiscordMemberId);

            return Task.FromResult(response);
        }
    }
}
