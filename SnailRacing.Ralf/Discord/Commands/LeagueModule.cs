using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SnailRacing.Ralf.Providers;

// ToDo: refactor all this to make it better, use fluent syntax for validation and processing of commands

namespace SnailRacing.Ralf.Discord.Commands
{
    [Group("league")]
    [Description("League administration made easy, subjective opinion, good luck getting registered!")]
    public class LeagueModule : BaseCommandModule
    {
        public AppConfig? AppConfig { get; set; }
        public IStorageProvider<LeagueStorageProviderModel>? StorageProvider { private get; set; }

        [Command("join")]
        public async Task JoinLeague(CommandContext ctx, string leagueName)
        {
            await ctx.TriggerTypingAsync();

            // ToDo: Really need to fix these Store and Internal store shenanigans
            if (!StorageProvider!.Store!.InternalStore!.ContainsKey(leagueName))
            {
                var noEntryEmoji = DiscordEmoji.FromName(ctx.Client, ":no_entry:");
                await ctx.RespondAsync($"{noEntryEmoji} The {leagueName} do not exist. Use !league for a list of active leagues you can join.");
                return;
            }

            var league = StorageProvider?.Store[leagueName];

            if (league!.Store.IsMember(ctx.Member))
            {
                await ctx.RespondAsync($"You are already a {league.Store[ctx.Member.Id.ToString()]?.Status} member of {leagueName}");
                return;
            }

            var joined = StorageProvider?.Store[leagueName]?.Join(ctx.Member, 0, string.Empty);

            await ctx.RespondAsync($"You were added to the {leagueName} league, your status is pending approval and a league admin will be in touch soon.");
        }

        [Command("new")]
        public async Task NewLeague(CommandContext ctx, string name, [RemainingText] string description)
        {
            await ctx.TriggerTypingAsync();

            // ToDo: need to sort the storage model out to fully encapsulate the InternalStore
            if (StorageProvider!.Store!.InternalStore!.ContainsKey(name))
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
            var leagues = StorageProvider!.Store
                .Select(x => new
                {
                    Name = x.Key,
                    x.Value.Description,
                    CreatedOn = x.Value.CreatedDate,
                    Pending = x.Value.Store.Count(p => p.Value.Status == LeagueParticipantStatus.Pending),
                    Approved = x.Value.Store.Count(p => p.Value.Status == LeagueParticipantStatus.Approved),
                    x.Value.Standings
                });

            foreach (var league in leagues)
            {
                var builder = new DiscordEmbedBuilder()
                    .WithTitle(league.Name)
                    .WithUrl(league.Standings)
                    .WithDescription(league.Description)
                    .WithColor(DiscordColor.DarkRed)
                    .AddField("Approved", league.Approved.ToString(), true)
                    .AddField("Pending", league.Pending.ToString(), true)
                    .AddField("Created On", league.CreatedOn.ToShortDateString());

                await ctx.Channel.SendMessageAsync(builder);
            }
        }
    }
}