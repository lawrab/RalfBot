using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SnailRacing.Ralf.DiscordCommands
{
    [Group("league")]
    [Description("League administration made easy, subjective opinion, good luck getting registered!")]
    public class LeagueModule : BaseCommandModule
    {
        [Command("new")]
        public async Task NewLeague(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync($"New league not implemented yet");
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
