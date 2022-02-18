using FluentValidation;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueNewRequestValidator : AbstractValidator<LeagueNewRequest>
    {
        private readonly IStorageProvider<LeagueStorageProviderModel> _storage;

        public LeagueNewRequestValidator(IStorageProvider<LeagueStorageProviderModel> storage)
        {
            _storage = storage;

            RuleFor(m => m.GuildId).NotEmpty();
            RuleFor(m => m.LeagueName)
                .NotEmpty()
                .Must(m => !_storage.Store.InternalStore!.ContainsKey(m))
                .WithMessage(m => $"League {m.LeagueName} already exist. Sorry, try again.");
        }
    }
}
