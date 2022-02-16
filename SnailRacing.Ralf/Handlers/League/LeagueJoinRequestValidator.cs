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
                validationContext.AddFailure($"The {leagueName} do not exist. Use !league for a list of active leagues you can join.");
            }
        }

        private void IsNotMemberOfLeague(string leagueName, ValidationContext<LeagueJoinRequest> validationContext)
        {
            var league = _storage.Store[leagueName];
            var discordMemberId = validationContext.InstanceToValidate.DiscordMemberId;
            if (league!.Store.IsMember(discordMemberId))
            {
                validationContext.AddFailure($"You are already a {league.Store[discordMemberId]?.Status} member of {leagueName}");
            }
        }
    }
}
