using MediatR;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueNewHandler : IRequestHandler<LeagueNewRequest, LeagueNewResponse>
    {
        private IStorageProvider _storageProvider;
        private readonly AppConfig _config;

        public LeagueNewHandler(IStorageProvider storageProvider,
                    AppConfig config)
        {
            _storageProvider = storageProvider;
            _config = config;
        }

        public Task<LeagueNewResponse> Handle(LeagueNewRequest request, CancellationToken cancellationToken)
        {
            var response = new LeagueNewResponse();

            var store = StoreHelper.GetLeagueStore(request.GuildId, _storageProvider);
            store.TryAdd(request.LeagueKey, new LeagueModel
            {
                Guild = request.GuildId,
                Name = request.LeagueName, 
                Description = request.Description,
                Status = LeagueStatus.Closed,
                CreatedDate = DateTime.UtcNow,
                Standings = new Uri("http://annieandlarry.com")
            });

            return Task.FromResult(response);
        }
    }
}
