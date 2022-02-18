using FluentValidation;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueLeaveRequestValidator : AbstractValidator<LeagueLeaveRequest>
    {
        private readonly IStorageProvider<LeagueStorageProviderModel> _storage;

        public LeagueLeaveRequestValidator(IStorageProvider<LeagueStorageProviderModel> storage)
        {
            _storage = storage;
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
            if (!_storage.Store.InternalStore!.ContainsKey(validationContext.InstanceToValidate.LeagueKey))
            {
                validationContext.AddFailure(string.Format(Messages.INVALID_LEAGUE, leagueName));
                return;
            }

            var league = _storage.Store[validationContext.InstanceToValidate.LeagueKey];
            if (league?.Name != leagueName)
            {
                validationContext.AddFailure(string.Format(Messages.INVALID_LEAGUE, leagueName));
            }
        }

        private void IsMemberOfLeague(string leagueName, ValidationContext<LeagueLeaveRequest> validationContext)
        {
            var league = _storage.Store[validationContext.InstanceToValidate.LeagueKey];
            var discordMemberId = validationContext.InstanceToValidate.DiscordMemberId;
            if (!league!.Store.IsMember(discordMemberId))
            {
                validationContext.AddFailure(string.Format(Messages.NOT_MEMBER_OF_LEAGUE, leagueName));
            }
        }
    }
}
