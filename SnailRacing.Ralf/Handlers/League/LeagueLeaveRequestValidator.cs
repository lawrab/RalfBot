using FluentValidation;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueLeaveRequestValidator : AbstractValidator<LeagueLeaveRequest>
    {
        private readonly IStorageProvider _storageProvider;

        public LeagueLeaveRequestValidator(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
            CascadeMode = CascadeMode.Stop;
            RuleFor(m => m.GuildId).NotEmpty();
            RuleFor(x => x.DiscordMemberId).NotEmpty();
            RuleFor(r => r.LeagueName)
                .NotEmpty()
                .Custom(IsValidLeagueName)
                .Custom(IsMemberOfLeague);
        }

        private void IsValidLeagueName(string leagueName, ValidationContext<LeagueLeaveRequest> validationContext)
        {
            var leagues = StoreHelper.GetLeagueStore(validationContext.InstanceToValidate.GuildId, _storageProvider);
            if (!leagues.ContainsKey(validationContext.InstanceToValidate.LeagueKey))
            {
                validationContext.AddFailure(string.Format(Messages.INVALID_LEAGUE, leagueName));
                return;
            }

            var league = leagues[validationContext.InstanceToValidate.LeagueKey];
            if (league?.Name != leagueName)
            {
                validationContext.AddFailure(string.Format(Messages.INVALID_LEAGUE, leagueName));
            }
        }

        private void IsMemberOfLeague(string leagueName, ValidationContext<LeagueLeaveRequest> validationContext)
        {
            var league = StoreHelper.GetLeague(validationContext.InstanceToValidate.GuildId, validationContext.InstanceToValidate.LeagueKey, _storageProvider);

            var discordMemberId = validationContext.InstanceToValidate.DiscordMemberId;
            if (!league.Participants.ContainsKey(discordMemberId))
            {
                validationContext.AddFailure(string.Format(Messages.NOT_MEMBER_OF_LEAGUE, leagueName));
            }
        }
    }
}
