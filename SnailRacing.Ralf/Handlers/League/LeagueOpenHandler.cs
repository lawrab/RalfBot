using MediatR;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueOpenHandler : IRequestHandler<LeagueOpenRequest, LeagueOpenResponse>
    {
        private readonly IStorageProvider _storageProvider;

        public LeagueOpenHandler(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public Task<LeagueOpenResponse> Handle(LeagueOpenRequest request, CancellationToken cancellationToken)
        {
            var store = StoreHelper.GetLeagueStore(request.GuildId, _storageProvider);
            var league = store[request.LeagueKey];
            league.Status = LeagueStatus.Open;
            league.MaxGrid = request.MaxGrid;

            store.TryUpdate(request.LeagueKey, league);

            return Task.FromResult(new LeagueOpenResponse());
        }
    }
}
