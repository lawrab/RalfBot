using MediatR;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueRemoveHandler : IRequestHandler<LeagueRemoveRequest, LeagueRemoveResponse>
    {
        private readonly IStorageProvider _storageProvider;

        public LeagueRemoveHandler(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public Task<LeagueRemoveResponse> Handle(LeagueRemoveRequest request, CancellationToken cancellationToken)
        {
            var store = StoreHelper.GetLeagueStore(request.GuildId, _storageProvider);
            store.TryRemove(request.LeagueKey);

            return Task.FromResult(new LeagueRemoveResponse());
        }
    }
}
