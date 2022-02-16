using FluentValidation;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueNewHandler : ICommand<LeagueNewRequest, LeagueNewResponse>
    {
        private IStorageProvider<LeagueStorageProviderModel> _storage;
        private IValidator<LeagueNewRequest> _validator;
        private readonly AppConfig _config;

        public LeagueNewHandler(IStorageProvider<LeagueStorageProviderModel> storage,
                    IValidator<LeagueNewRequest> validator,
                    AppConfig config)
        {
            _storage = storage;
            _validator = validator;
            _config = config;
        }

        public Task<LeagueNewResponse> Handle(LeagueNewRequest request)
        {
            var validationResponse = _validator.Validate(request);

            var response = new LeagueNewResponse
            {
                Errors = validationResponse.Errors.Select(e => e.ErrorMessage)
            };

            _storage.Store[request.LeagueName] = new LeagueModel(request.LeagueName, 
                request.Description, 
                DateTime.UtcNow, 
                _config.DataPath ?? string.Empty);

            return Task.FromResult(response);
        }
    }
}
