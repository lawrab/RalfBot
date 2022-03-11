using FluentValidation;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueRemoveRequestValidator : AbstractValidator<LeagueRemoveRequest>
    {
        private readonly IStorageProvider _storageProvider;

        public LeagueRemoveRequestValidator(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
            CascadeMode = CascadeMode.Stop;
            RuleFor(r => r.LeagueName)
                .NotEmpty()
                .Must((r, _) => StoreHelper.GetLeagueStore(r.GuildId, _storageProvider)[r.LeagueKey] != null)
                .WithMessage(m => $"League {m.LeagueName} do not exist. Deleting the void could result in unpredictable consequences. Try !league to see all leagues.");
        }
    }
}
