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
        public AppConfig? AppConfig { get; set; }
        public IStorageProvider<LeagueStorageProviderModel>? StorageProvider { private get; set; }

        [Command("new")]
        public async Task NewLeague(CommandContext ctx, string name, [RemainingText] string description)
        {
            await ctx.TriggerTypingAsync();

            // ToDo: need to sort the storage model out to fully encapsulate the InternalStore
            if(StorageProvider!.Store!.InternalStore!.ContainsKey(name))
            {
                var noEntryEmoji = DiscordEmoji.FromName(ctx.Client, ":no_entry:");
                await ctx.RespondAsync($"{noEntryEmoji} League {name} already exist. Sorry, try again.");
                return;
            }

            StorageProvider!.Store[name] = new LeagueModel(name, description, DateTime.UtcNow, AppConfig?.DataPath ?? string.Empty);

            var successEmoji = DiscordEmoji.FromName(ctx.Client, ":ok:", true);
            await ctx.RespondAsync($"{successEmoji} New league {name} created, you can win this!");
        }

        [Command("remove")]
        public async Task RemoveLeague(CommandContext ctx, string leagueName)
        {
            await ctx.TriggerTypingAsync();

            var removed = StorageProvider!.Store.Remove(leagueName);
            var responseMessage = removed ? $"League **{leagueName}** wiped from the end of the earth." : $"Mistakes were made, **{leagueName}** isn't a thing, try !league to see all leagues.";

            await ctx.RespondAsync(responseMessage);
        }

        [GroupCommand]
        public async Task ListLeagues(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var leagues  = StorageProvider!.Store.Select(x => $"**{x.Key}**: `{x.Value.Description}` created on `{x.Value.CreatedDate.ToShortDateString()}`");

            var builder = new DiscordMessageBuilder();
            builder.WithReply(ctx.Message.Id)
                .WithContent("_All leagues_")
                .WithContent(string.Join(Environment.NewLine, leagues));
            
            await ctx.RespondAsync(builder);
        }
    }
}
