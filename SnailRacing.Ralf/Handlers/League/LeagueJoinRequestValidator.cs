using FluentValidation;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueJoinRequestValidator : AbstractValidator<LeagueJoinRequest>
    {
        private readonly IStorageProvider<LeagueStorageProviderModel> _storage;

        public LeagueJoinRequestValidator(IStorageProvider<LeagueStorageProviderModel> storage)
        {
            _storage = storage;

            CascadeMode = CascadeMode.Stop;

            RuleFor(r => r.LeagueName)
                .NotEmpty()
                .Custom(IsValidLeagueName)
                .Custom(IsNotMemberOfLeague);
        }

        private void IsValidLeagueName(string leagueName, ValidationContext<LeagueJoinRequest> validationContext)
        {
            if (_storage.Store.InternalStore?.ContainsKey(leagueName) != true)
            {
                validationContext.AddFailure(string.Format(Messages.INVALID_LEAGUE, leagueName));
            }
        }

        private void IsNotMemberOfLeague(string leagueName, ValidationContext<LeagueJoinRequest> validationContext)
        {
            var league = _storage.Store[leagueName];
            var discordMemberId = validationContext.InstanceToValidate.DiscordMemberId;
            if (league!.Store.IsMember(discordMemberId))
            {
                validationContext.AddFailure( string.Format(Messages.ALREADY_MEMBER_OF_LEAGUE, league.Store[discordMemberId]?.Status, leagueName));
            }
        }
    }
}
