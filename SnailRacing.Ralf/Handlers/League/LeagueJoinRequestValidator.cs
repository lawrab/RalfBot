using FluentValidation;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueJoinRequestValidator : AbstractValidator<LeagueJoinRequest>
    {
        private readonly IStorageProvider _storageProvider;

        public LeagueJoinRequestValidator(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;

            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.GuildId).NotEmpty();
            RuleFor(x => x.DiscordMemberId).NotEmpty();
            RuleFor(r => r.LeagueName)
                .NotEmpty()
                .Custom(IsValidLeagueName)
                .Custom(IsNotMemberOfLeague);
        }

        private void IsValidLeagueName(string leagueName, ValidationContext<LeagueJoinRequest> validationContext)
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

        private void IsNotMemberOfLeague(string leagueName, ValidationContext<LeagueJoinRequest> validationContext)
        {
            var league = StoreHelper.GetLeague(validationContext.InstanceToValidate.GuildId, validationContext.InstanceToValidate.LeagueKey, _storageProvider);

            var discordMemberId = validationContext.InstanceToValidate.DiscordMemberId;
            if (league.Participants.ContainsKey(discordMemberId))
            {
                validationContext.AddFailure( string.Format(Messages.ALREADY_MEMBER_OF_LEAGUE, league.Participants[discordMemberId]?.Status, leagueName));
            }
        }
    }
}
