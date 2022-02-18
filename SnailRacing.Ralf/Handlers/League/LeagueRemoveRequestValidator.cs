using FluentValidation;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueRemoveRequestValidator : AbstractValidator<LeagueRemoveRequest>
    {
        private readonly IStorageProvider<LeagueStorageProviderModel> _storage;

        public LeagueRemoveRequestValidator(IStorageProvider<LeagueStorageProviderModel> storage)
        {
            _storage = storage;
            RuleFor(r => r.LeagueName)
                .NotEmpty()
                .Must((r, _) => _storage.Store.InternalStore!.ContainsKey(r.LeagueKey))
                .WithMessage(m => $"League {m.LeagueName} do not exist. Deleting the void could result in unpredictable consequences. Try !league to see all leagues.");
        }
    }
}
