using MediatR;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

//ToDo: refactor and rename to be a more generic remove handler
namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueLeaveHandler : IRequestHandler<LeagueLeaveRequest, LeagueLeaveResponse>
    {
        private readonly IStorageProvider _storageProvider;

        public LeagueLeaveHandler(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public Task<LeagueLeaveResponse> Handle(LeagueLeaveRequest request, CancellationToken cancellationToken)
        {
            var response = new LeagueLeaveResponse();

            var leagues = StoreHelper.GetLeagueStore(request.GuildId, _storageProvider);
            var league = leagues[request.LeagueKey];

            league.Participants.Remove(request.DiscordMemberId, out _);

            leagues.TryUpdate(request.LeagueKey, league);

            return Task.FromResult(response);
        }
    }
}
