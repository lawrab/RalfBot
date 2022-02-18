using MediatR;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueRemoveHandler : IRequestHandler<LeagueRemoveRequest, LeagueRemoveResponse>
    {
        private readonly IStorageProvider<LeagueStorageProviderModel> _storage;

        public LeagueRemoveHandler(IStorageProvider<LeagueStorageProviderModel> storage)
        {
            _storage = storage;
        }

        public Task<LeagueRemoveResponse> Handle(LeagueRemoveRequest request, CancellationToken cancellationToken)
        {
            _storage!.Store.Remove(request.LeagueKey);

            return Task.FromResult(new LeagueRemoveResponse());
        }
    }
}
