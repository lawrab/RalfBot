using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.DiscordCommands
{
    [Group("league")]
    [Description("League administration made easy, subjective opinion, good luck getting registered!")]
    public class LeagueModule : BaseCommandModule
    {
        public IStorageProvider<LeagueStorageProviderModel>? StorageProvider { private get; set; }

        [Command("new")]
        public async Task NewLeague(CommandContext ctx, string name, [RemainingText] string description)
        {
            await ctx.TriggerTypingAsync();

            if(StorageProvider!.Store.InternalStore.ContainsKey(name))
            {
                var noEntryEmoji = DiscordEmoji.FromName(ctx.Client, ":no_entry:");
                await ctx.RespondAsync($"{noEntryEmoji} League {name} already exist. Sorry, try again.");
                return;
            }

            // ToDo: create propper constructors for the models to default values
            StorageProvider!.Store[name] = new LeagueModel
            {
                Name = name,
                Description = description,
                CreatedDate = DateTime.UtcNow
            };

            var successEmoji = DiscordEmoji.FromName(ctx.Client, ":ok:", true);
            await ctx.RespondAsync($"{successEmoji} New league {name} created, you can win this!");
        }

        [Command("remove")]
        public async Task RemoveLeague(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync($"Remove league not implemented yet");
        }

        [GroupCommand]
        public async Task ListLeagues(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync($"List leagues not implemented yet");
        }
    }
}
