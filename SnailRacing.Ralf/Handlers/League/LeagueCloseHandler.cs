using MediatR;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueCloseHandler : IRequestHandler<LeagueCloseRequest, LeagueCloseResponse>
    {
        private readonly IStorageProvider<LeagueStorageProviderModel> _storage;

        public LeagueCloseHandler(IStorageProvider<LeagueStorageProviderModel> storage)
        {
            _storage = storage;
        }

        public Task<LeagueCloseResponse> Handle(LeagueCloseRequest request, CancellationToken cancellationToken)
        {
            _storage.Store.SetClosed(request.LeagueKey);

            return Task.FromResult(new LeagueCloseResponse());
        }
    }
}
