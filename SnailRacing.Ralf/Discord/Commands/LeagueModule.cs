using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MediatR;
using SnailRacing.Ralf.Handlers.League;
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
        public IMediator? Mediator { get; set; }

        [Command("join")]
        public async Task JoinLeague(CommandContext ctx, string leagueName)
        {
            await ctx.TriggerTypingAsync();

            var response = await Mediator!.Send(new LeagueJoinRequest
            {
                DiscordMemberId = ctx.Member.Id.ToString(),
                LeagueName = leagueName
            });

            if (response.HasErrors())
            {
                var errorEmoji = DiscordEmoji.FromName(ctx.Client, ":no_entry:", true);
                await ctx.RespondAsync($"{errorEmoji} {string.Join(Environment.NewLine, response.Errors)}");
                return;
            }

            await ctx.RespondAsync($"You were added to the {leagueName} league, your status is pending approval and a league admin will be in touch soon.");
        }

        [Command("new")]
        public async Task NewLeague(CommandContext ctx, string leagueName, [RemainingText] string description)
        {
            await ctx.TriggerTypingAsync();

            var response = await Mediator!.Send(new LeagueNewRequest
            {
                LeagueName = leagueName,
                Description = description
            });

            if(response.HasErrors())
            {
                var errorEmoji = DiscordEmoji.FromName(ctx.Client, ":no_entry:", true);
                await ctx.RespondAsync($"{errorEmoji} {string.Join(Environment.NewLine, response.Errors)}");
                return;
            }

            var successEmoji = DiscordEmoji.FromName(ctx.Client, ":ok:", true);
            await ctx.RespondAsync($"{successEmoji} New league {leagueName} created, you can win this!");
        }

        [Command("remove")]
        public async Task RemoveLeague(CommandContext ctx, string leagueName)
        {

            await ctx.TriggerTypingAsync();

            var response = await Mediator!.Send(new LeagueRemoveRequest
            {
                LeagueName = leagueName,
            });

            if (response.HasErrors())
            {
                var errorEmoji = DiscordEmoji.FromName(ctx.Client, ":no_entry:", true);
                await ctx.RespondAsync($"{errorEmoji} {string.Join(Environment.NewLine, response.Errors)}");
                return;
            }

            await ctx.RespondAsync($"League **{leagueName}** wiped from the end of the earth.");
        }

        [GroupCommand]
        public async Task ListLeagues(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var response = await Mediator!.Send(new LeagueQueryRequest());

            if (response.HasErrors())
            {
                var errorEmoji = DiscordEmoji.FromName(ctx.Client, ":no_entry:", true);
                await ctx.RespondAsync($"{errorEmoji} {string.Join(Environment.NewLine, response.Errors)}");
                return;
            }

            if(!response.Leagues.Any())
            {
                var shrugEmoji = DiscordEmoji.FromName(ctx.Client, ":shrug:", true);
                await ctx.RespondAsync($"{shrugEmoji} I cannot find any leagues, add one if you want to see something here!");
                return;
            }

            var leagues = response.Leagues
                .Select(x => new
                {
                    Name = x.Name,
                    x.Description,
                    CreatedOn = x.CreatedDate,
                    Pending = x.Store.Count(p => p.Value.Status == LeagueParticipantStatus.Pending),
                    Approved = x.Store.Count(p => p.Value.Status == LeagueParticipantStatus.Approved),
                    x.Standings
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