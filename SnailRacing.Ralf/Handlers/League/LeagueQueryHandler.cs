using MediatR;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueQueryHandler : IRequestHandler<LeagueQueryRequest, LeagueQueryResponse>
    {
        private readonly IStorageProvider<LeagueStorageProviderModel> _storage;

        public LeagueQueryHandler(IStorageProvider<LeagueStorageProviderModel> storage)
        {
            _storage = storage;
        }
        public Task<LeagueQueryResponse> Handle(LeagueQueryRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new LeagueQueryResponse
            {
                Leagues = _storage.Store.Where(x => request.Query(x.Value)).Select(x => x.Value)
            });
        }
    }
}
