using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueJoinHandler : ICommand<LeagueJoinRequest, LeagueJoinResponse>
    {
        private readonly IStorageProvider<LeagueStorageProviderModel> _storage;
        private readonly IValidate<LeagueJoinRequest> _validator;

        public LeagueJoinHandler(IStorageProvider<LeagueStorageProviderModel> storage,
            IValidate<LeagueJoinRequest> validator)
        {
            _storage = storage;
            _validator = validator;
        }
        public Task<LeagueJoinResponse> Handle(LeagueJoinRequest request)
        {
            throw new NotImplementedException();

            ////// ToDo: Really need to fix these Store and Internal store shenanigans
            ////if (!StorageProvider!.Store!.InternalStore!.ContainsKey(leagueName))
            ////{
            ////    var noEntryEmoji = DiscordEmoji.FromName(ctx.Client, ":no_entry:");
            ////    await ctx.RespondAsync($"{noEntryEmoji} The {leagueName} do not exist. Use !league for a list of active leagues you can join.");
            ////    return;
            ////}

            ////var league = StorageProvider?.Store[leagueName];

            ////if (league!.Store.IsMember(ctx.Member))
            ////{
            ////    await ctx.RespondAsync($"You are already a {league.Store[ctx.Member.Id.ToString()]?.Status} member of {leagueName}");
            ////    return;
            ////}

            ////var joined = StorageProvider?.Store[leagueName]?.Join(ctx.Member, 0, string.Empty);

            ////await ctx.RespondAsync($"You were added to the {leagueName} league, your status is pending approval and a league admin will be in touch soon.");

        }
    }
}
