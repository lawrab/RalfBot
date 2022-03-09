using MediatR;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueCloseHandler : IRequestHandler<LeagueCloseRequest, LeagueCloseResponse>
    {
        private readonly IStorageProvider _storageProvider;

        public LeagueCloseHandler(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public Task<LeagueCloseResponse> Handle(LeagueCloseRequest request, CancellationToken cancellationToken)
        {
            var store = StoreHelper.GetLeagueStore(request.GuildId, _storageProvider);
            var league = store[request.LeagueKey];
            league.Status = LeagueStatus.Closed;

            store.TryUpdate(request.LeagueKey, league);

            return Task.FromResult(new LeagueCloseResponse());
        }
    }
}
