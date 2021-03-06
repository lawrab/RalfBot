using FluentValidation;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueCloseRequestValidator : AbstractValidator<LeagueCloseRequest>
    {
        private readonly IStorageProvider _storageProvider;

        public LeagueCloseRequestValidator(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
            RuleFor(r => r.GuildId)
                .NotEmpty();
            RuleFor(r => r.LeagueName)
                           .NotEmpty()
                           .Must((r, _) => StoreHelper.GetLeagueStore(r.GuildId, _storageProvider)[r.LeagueKey] != null)
                           .WithMessage(m => $"League {m.LeagueName} do not exist. Deleting the void could result in unpredictable consequences. Try !league to see all leagues.");
        }
    }
}
