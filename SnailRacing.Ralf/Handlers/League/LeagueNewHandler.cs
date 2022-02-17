using MediatR;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueNewHandler : IRequestHandler<LeagueNewRequest, LeagueNewResponse>
    {
        private IStorageProvider<LeagueStorageProviderModel> _storage;
        private readonly AppConfig _config;

        public LeagueNewHandler(IStorageProvider<LeagueStorageProviderModel> storage,
                    AppConfig config)
        {
            _storage = storage;
            _config = config;
        }

        public Task<LeagueNewResponse> Handle(LeagueNewRequest request, CancellationToken cancellationToken)
        {
            var response = new LeagueNewResponse();

            _storage.Store[request.LeagueName] = new LeagueModel(request.LeagueName,
                request.Description,
                DateTime.UtcNow,
                _config.DataPath ?? string.Empty);

            return Task.FromResult(response);
        }
    }
}
