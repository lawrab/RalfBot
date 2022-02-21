using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using MediatR;
using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;


//ToDo: move to ctor injection instead of property injection
namespace SnailRacing.Ralf.Discord.Commands
{
    [Group("league")]
    [Aliases("leagues")]
    [RequireGuild]
    [Description("League administration made easy, subjective opinion, good luck getting registered!")]
    public class LeagueModule : BaseCommandModule
    {
        private const string ADMIN_ROLE = "League Admin";
        public AppConfig? AppConfig { get; set; }
        public IStorageProvider<LeagueStorageProviderModel>? StorageProvider { private get; set; }
        public IMediator? Mediator { get; set; }

        [Command("join")]
        [Description("Request to join a league")]
        public async Task JoinLeague(CommandContext ctx,
            [Description("Use !league to get a list of leagues")] string leagueName)
        {
            await ctx.TriggerTypingAsync();

            var request = await BuildLeagueJoinRequest(ctx, leagueName);

            if (request is null) return;
            if(!request.AgreeTermsAndConditions)
            {
                await ctx.Member.SendMessageAsync("I am sorry, you cannot join the league unless you agree with the Snail Racing code of conduct.");
                return;
            }

            if (request is null) return;

            var response = await Mediator!.Send(request);

            var responseMessage = response
                .ToResponseMessage($"Thank you, you were added to the **{leagueName}** league, your status is pending approval and a league admin will be in touch soon.");

            await ctx.Member.SendMessageAsync(responseMessage);
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
        [RequireRoles(RoleCheckMode.Any, ADMIN_ROLE)]
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

        [Command("status")]
        [Description("Shows the status for the league")]
        public async Task LeagueStatus(CommandContext ctx, string leagueName)
        {
            await ctx.TriggerTypingAsync();

            var response = await Mediator!.Send(new LeagueQueryRequest
            {
                GuildId = ctx.Guild.Id.ToString(),
                Query = (l) => l.Name == leagueName
            });

            if (response.HasErrors())
            {
                await ctx.RespondAsync(response.ToErrorMessage());
                return;
            }

            if (!response.Leagues.Any())
            {
                await ctx.RespondAsync(string.Format(Messages.INVALID_LEAGUE, leagueName));
                return;
            }

            var league = response.Leagues.SingleOrDefault();

            var waitingListParticipants = league?.Store.Count(p => p.Value.Status == LeagueParticipantStatus.Pending);
            var activeParticipants = league?.Store.Count(p => p.Value.Status == LeagueParticipantStatus.Approved);

            var builder = new DiscordEmbedBuilder()
                    .WithTitle(league?.Name)
                    .WithUrl(league?.Standings)
                    .WithDescription(league?.Description)
                    .WithColor(DiscordColor.DarkRed)
                    .AddField("Max cars", league?.MaxGrid.ToString(), true)
                    .AddField("Drivers", activeParticipants.ToString(), true)
                    .AddField("Waiting List", waitingListParticipants.ToString(), true)
                    .AddField("Created On", league?.CreatedDate.ToShortDateString());

            await ctx.Channel.SendMessageAsync(builder);
        }

        [Command("open")]
        [Description("Set the league to open for automatic approval")]
        public async Task SetLeagueOpen(CommandContext ctx, 
            string leagueName,
            [Description("Maximum grid positions, also the maximum number of automatic approvals before the league is closed")] int gridSpots)
        {
            await ctx.TriggerTypingAsync();

            var response = await Mediator!.Send(new LeagueOpenRequest
            {
                GuildId = ctx.Guild.Id.ToString(),
                LeagueName = leagueName,
                MaxGrid = gridSpots
            });

            if (response.HasErrors())
            {
                await ctx.RespondAsync(response.ToErrorMessage());
                return;
            }

            await ctx.RespondAsync($"League **{leagueName}** now open with auto approval up to **{gridSpots}**.");
        }

        [Command("close")]
        [Description("Set the league to closed for automatic approval")]
        public async Task SetLeagueClosed(CommandContext ctx, string leagueName)
        {
            await ctx.TriggerTypingAsync();
        }

        [Command("participants")]
        public async Task LeagueParticipants(CommandContext ctx, string leagueName)
        {
            await LeagueParticipants(ctx, leagueName, "all");
        }

        [Command("participants")]
        [Aliases("part", "member", "reg")]
        [Description("Shows the registered members for the league")]
        public async Task LeagueParticipants(CommandContext ctx, string leagueName,
        [Description("Use, `drivers, waiting, banned` to filter the results, default is all drivers.")] string filter)
        {
            await ctx.TriggerTypingAsync();

            var response = await Mediator!.Send(new LeagueQueryRequest
            {
                GuildId = ctx.Guild.Id.ToString(),
                Query = (l) => l.Name == leagueName
            });

            if (response.HasErrors())
            {
                await ctx.RespondAsync(response.ToErrorMessage());
                return;
            }

            if (!response.Leagues.Any())
            {
                await ctx.RespondAsync(string.Format(Messages.INVALID_LEAGUE, leagueName));
                return;
            }
            var league = response.Leagues.SingleOrDefault();

            var filterExpression = filter.ToLower() switch
            {
                "drivers" => new Predicate<LeagueParticipantModel>(p => p.Status == LeagueParticipantStatus.Approved),
                "waiting" => new Predicate<LeagueParticipantModel>(p => p.Status == LeagueParticipantStatus.Pending),
                "banned" => new Predicate<LeagueParticipantModel>(p => p.Status == LeagueParticipantStatus.Banned),
                _ => new Predicate<LeagueParticipantModel>(x => true)
            };

            var drivers = league?.Store.Where(p => filterExpression(p.Value)).Select(p => p.Value);

            if (drivers?.Any() == false)
            {
                await ctx.RespondAsync(":shrug: No drivers found for your query");
                return;
            }

            foreach (var driver in drivers ?? Enumerable.Empty<LeagueParticipantModel>())
            {
                var member = await ctx.Guild.GetMemberAsync(ulong.Parse(driver.DiscordMemberId));
                var builder = new DiscordEmbedBuilder()
                        .WithTitle($"{league?.Name} - {member.DisplayName}")
                        .WithDescription($"**{driver.IRacingName}**")
                        .WithColor(DiscordColor.DarkRed)
                        .AddField("iRacing Customer ID", driver.IRacingCustomerId.ToString(), true)
                        .AddField("Registration Date", driver.RegistrationDate.GetValueOrDefault().ToShortDateString(), true)
                        .AddField("Status", driver.Status.ToString(), true);

                await ctx.Channel.SendMessageAsync(builder);
            }
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
                    .AddField("Drivers", league.Approved.ToString(), true)
                    .AddField("Waiting List", league.Pending.ToString(), true)
                    .AddField("Created On", league.CreatedOn.ToShortDateString());

                await ctx.Channel.SendMessageAsync(builder);
            }
        }

        private async Task<LeagueJoinRequest?> BuildLeagueJoinRequest(CommandContext ctx, string leagueName)
        {
            var interactivity = ctx.Client.GetInteractivity();
            await ctx.Member.SendMessageAsync("We need a bit more information ");

            var msg = await ctx.Member.SendMessageAsync("What is your name? (As used in iRacing.)");

            var response = await interactivity.WaitForMessageAsync((m) => m.ChannelId == msg.ChannelId && m.Author == ctx.Member);

            if (response.TimedOut) return null;
            var name = response.Result.Content;

            msg = await ctx.Member.SendMessageAsync("What is your iRacing Customer ID? (Optional, respond with `none` fi you don't know)");
            response = await interactivity.WaitForMessageAsync((m) => m.ChannelId == msg.ChannelId && m.Author == ctx.Member); ;
            if (response.TimedOut) return null;
            if (!int.TryParse(response.Result.Content, out int customerId))
            {
                customerId = -1;
            }

            msg = await ctx.Member.SendMessageAsync("Have you read and agree with the Snail Racing code of conduct `yes/no` (https://annieandlarry.com/snail-racing-conduct)");
            response = await interactivity.WaitForMessageAsync((m) => m.ChannelId == msg.ChannelId && m.Author == ctx.Member);
            if (response.TimedOut) return null;
            if (!bool.TryParse(response.Result.Content, out bool agreeTermsAndConditions))
            {
                agreeTermsAndConditions = false;
            }

            var request = new LeagueJoinRequest
            {
                GuildId = ctx.Guild.Id.ToString(),
                DiscordMemberId = ctx.Member.Id.ToString(),
                LeagueName = leagueName,
                IRacingName = name,
                IRacingCustomerId = customerId,
                AgreeTermsAndConditions = agreeTermsAndConditions
            };

            return request;
        }
    }
}