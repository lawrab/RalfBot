using FluentValidation;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueNewRequestValidator : AbstractValidator<LeagueNewRequest>
    {
        private readonly IStorageProvider _storageProvider;

        public LeagueNewRequestValidator(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;

            RuleFor(m => m.GuildId).NotEmpty();
            RuleFor(m => m.LeagueName)
                .NotEmpty()
                .Must((m, _) => StoreHelper.GetLeagueStore(m.GuildId, _storageProvider)[m.LeagueKey] != null)
                .WithMessage(m => $"League {m.LeagueName} already exist. Sorry, try again.");
        }
    }
}
