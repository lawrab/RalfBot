using MediatR;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueOpenHandler : IRequestHandler<LeagueOpenRequest, LeagueOpenResponse>
    {
        private readonly IStorageProvider<LeagueStorageProviderModel> _storage;

        public LeagueOpenHandler(IStorageProvider<LeagueStorageProviderModel> storage)
        {
            _storage = storage;
        }

        public Task<LeagueOpenResponse> Handle(LeagueOpenRequest request, CancellationToken cancellationToken)
        {
            _storage.Store.SetOpen(request.LeagueKey, request.MaxGrid);

            return Task.FromResult(new LeagueOpenResponse());
        }
    }
}
