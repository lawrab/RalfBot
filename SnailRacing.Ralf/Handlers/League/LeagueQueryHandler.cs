using MediatR;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueQueryHandler : IRequestHandler<LeagueQueryRequest, LeagueQueryResponse>
    {
        private readonly IStorageProvider _storageProvider;

        public LeagueQueryHandler(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }
        public Task<LeagueQueryResponse> Handle(LeagueQueryRequest request, CancellationToken cancellationToken)
        {
            var response = new LeagueQueryResponse
            {
                Leagues = StoreHelper.GetLeagueStore(request.GuildId, _storageProvider)
                .Where(x => request.Query(x.Value)).Select(x => x.Value)
            };
            return Task.FromResult(response);
        }
    }
}
