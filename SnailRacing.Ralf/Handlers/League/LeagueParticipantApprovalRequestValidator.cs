using FluentValidation;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueParticipantApprovalRequestValidator : AbstractValidator<LeagueParticipantApproalRequest>
    {
        private readonly IStorageProvider _storageProvider;

        public LeagueParticipantApprovalRequestValidator(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;

            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.GuildId).NotEmpty();
            RuleFor(x => x.DiscordMemberId).NotEmpty();
            RuleFor(x => x.ApprovedBy).NotEmpty();
            RuleFor(r => r.LeagueName)
                .NotEmpty()
                .Custom(IsValidLeagueName)
                .Custom(IsMemberOfLeague);
        }

        private void IsValidLeagueName(string leagueName, ValidationContext<LeagueParticipantApproalRequest> validationContext)
        {
            var store = StoreHelper.GetLeagueStore(validationContext.InstanceToValidate.GuildId, _storageProvider);

            if (store[validationContext.InstanceToValidate.LeagueKey] == null)
            {
                validationContext.AddFailure(string.Format(Messages.INVALID_LEAGUE, leagueName));
                return;
            }

            var league = store[validationContext.InstanceToValidate.LeagueKey];
            if (league?.Name != leagueName)
            {
                validationContext.AddFailure(string.Format(Messages.INVALID_LEAGUE, leagueName));
            }
        }

        private void IsMemberOfLeague(string leagueName, ValidationContext<LeagueParticipantApproalRequest> validationContext)
        {
            var store = StoreHelper.GetLeagueStore(validationContext.InstanceToValidate.GuildId, _storageProvider);
            var league = store[validationContext.InstanceToValidate.LeagueKey];
            var discordMemberId = validationContext.InstanceToValidate.DiscordMemberId;
            if (!league.Participants.ContainsKey(discordMemberId))
            {
                validationContext.AddFailure(string.Format(Messages.NOT_MEMBER_OF_LEAGUE, league.Participants[discordMemberId]?.Status, leagueName));
            }
        }
    }
}
