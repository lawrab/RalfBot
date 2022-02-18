using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MediatR;
using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Discord.Commands
{
    [Group("league")]
    [Aliases("leagues")]
    [RequireGuild]
    [Description("League administration made easy, subjective opinion, good luck getting registered!")]
    public class LeagueModule : BaseCommandModule
    {
        public AppConfig? AppConfig { get; set; }
        public IStorageProvider<LeagueStorageProviderModel>? StorageProvider { private get; set; }
        public IMediator? Mediator { get; set; }

        [Command("join")]
        [Description("Request to join a league")]
        public async Task JoinLeague(CommandContext ctx,
            [Description("Use !league to get a list of leagues")] string leagueName)
        {
            await ctx.TriggerTypingAsync();

            var response = await Mediator!.Send(new LeagueJoinRequest
            {
                GuildId = ctx.Guild.Id.ToString(),
                DiscordMemberId = ctx.Member.Id.ToString(),
                LeagueName = leagueName
            });

            var responseMessage = response
                .ToResponseMessage($"You were added to the {leagueName} league, your status is pending approval and a league admin will be in touch soon.");

            await ctx.RespondAsync(responseMessage);
        }

        [Command("leave")]
        [Description("Leave a league")]
        public async Task LeaveLeague(CommandContext ctx,
            [Description("Use !league to get a list of leagues")] string leagueName)
        {
            await ctx.TriggerTypingAsync();

            var response = await Mediator!.Send(new LeagueLeaveRequest
            {
                GuildId = ctx.Guild.Id.ToString(),
                DiscordMemberId = ctx.Member.Id.ToString(),
                LeagueName = leagueName
            });

            var responseMessage = response
                .ToResponseMessage($"You were removed from the {leagueName} league and can not longer participate in it.");

            await ctx.RespondAsync(responseMessage);
        }

        [Command("new")]
        [Aliases("add")]
        [Description("Creates a new league")]
        [RequireRoles(RoleCheckMode.Any, "League Admin")]
        public async Task NewLeague(CommandContext ctx,
            [Description("Call it something nice")] string leagueName,
            [Description("A short desrciption of the league")][RemainingText] string description)
        {
            await ctx.TriggerTypingAsync();

            var response = await Mediator!.Send(new LeagueNewRequest
            {
                GuildId = ctx.Guild.Id.ToString(),
                LeagueName = leagueName,
                Description = description
            });

            var responseMessage = response
                .ToResponseMessage($"New league {leagueName} created, you can win this!");

            await ctx.RespondAsync(responseMessage);
        }

        [Command("remove")]
        [Description("Deletes the league")]
        public async Task RemoveLeague(CommandContext ctx, string leagueName)
        {

            await ctx.TriggerTypingAsync();

            var response = await Mediator!.Send(new LeagueRemoveRequest
            {
                GuildId = ctx.Guild.Id.ToString(),
                LeagueName = leagueName,
            });

            var responseMessage = response
                .ToResponseMessage($"League **{leagueName}** wiped from the end of the earth.");

            await ctx.RespondAsync(responseMessage);
        }

        [GroupCommand]
        [Command("list")]
        [Description("Shows all the leagues")]
        public async Task ListLeagues(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var response = await Mediator!.Send(new LeagueQueryRequest
            {
                GuildId = ctx.Guild.Id.ToString(),
            });

            if (response.HasErrors())
            {
                await ctx.RespondAsync(response.ToErrorMessage());
                return;
            }

            if (!response.Leagues.Any())
            {
                var shrugEmoji = DiscordEmoji.FromName(ctx.Client, ":shrug:", true);
                await ctx.RespondAsync($"{shrugEmoji} There are currently no leagues running.");
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