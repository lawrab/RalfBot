using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Logging;

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
        private readonly ILogger<LeagueModule> _logger;

        public AppConfig? AppConfig { get; set; }
        public IMediator? Mediator { get; set; }

        public LeagueModule(ILogger<LeagueModule> logger)
        {
            _logger = logger;
        }

        [Command("join")]
        [Description("Request to join a league")]
        public async Task JoinLeague(CommandContext ctx,
            [Description("Use !league to get a list of leagues")] string leagueName)
        {
            using (LoggingHelper.BeginScope(_logger, ctx))
            {

                _logger.LogDebug("Command: !league join {leagueName} by {member}", leagueName, ctx.Member.DisplayName);
                await ctx.TriggerTypingAsync();

                var request = await BuildLeagueJoinRequest(ctx, leagueName);

                if (request is null) return;
                if (!request.AgreeTermsAndConditions)
                {
                    await ctx.Member.SendMessageAsync("I am sorry, you cannot join the league unless you agree with the Snail Racing code of conduct.");
                    return;
                }

                if (request is null) return;

                var response = await Mediator!.Send(request);

                var responseMessage = response
                    .ToResponseMessage($"Thank you, you were added to the **{leagueName}** league, a league admin will be in touch soon.");
                await ctx.Member.SendMessageAsync(responseMessage);

                // ToDo: improve this to ensure you can be configurable and the role is mentionable
                var adminRole = ctx.Guild.Roles.Where(r => r.Value.Name == ADMIN_ROLE).FirstOrDefault();

                await ctx.RespondAsync($"New member {ctx.Member.Mention} joined {leagueName}, {adminRole.Value.Mention}");

                if (response.MaxApprovedReached)
                {
                    await ctx.RespondAsync($"The {leagueName} have reached the maximum number of drivers and new registrations will be added to the waiting list");
                }
            }
        }

        [Command("leave")]
        [Description("Leave a league")]
        public async Task LeaveLeague(CommandContext ctx,
            [Description("Use !league to get a list of leagues")] string leagueName)
        {
            using (LoggingHelper.BeginScope(_logger, ctx))
            {
                _logger.LogDebug("Command: !league leave {leagueName} by {member}", leagueName, ctx.Member.DisplayName);
                await ctx.TriggerTypingAsync();

                var response = await Mediator!.Send(new LeagueLeaveRequest
                {
                    GuildId = ctx.Guild.Id.ToString(),
                    DiscordMemberId = ctx.Member.Id.ToString(),
                    LeagueName = leagueName
                });

                var responseMessage = response
                    .ToResponseMessage($"You were removed from the {leagueName} league and can no longer participate in it.");

                await ctx.RespondAsync(responseMessage);
            }
        }

        [Command("new")]
        [Aliases("add")]
        [Description("Creates a new league")]
        [RequireRoles(RoleCheckMode.Any, ADMIN_ROLE)]
        public async Task NewLeague(CommandContext ctx,
            [Description("Call it something nice")] string leagueName,
            [Description("A short desrciption of the league")][RemainingText] string description)
        {
            using (LoggingHelper.BeginScope(_logger, ctx))
            {
                _logger.LogDebug("Command: !league new/add {leagueName} by {member}", leagueName, ctx.Member.DisplayName);
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
        }

        [Command("kick")]
        [Description("Remove a participate in the league, they will need to join again")]
        [RequireRoles(RoleCheckMode.Any, ADMIN_ROLE)]
        public async Task KickParticipant(CommandContext ctx,
        [Description("The league where the driver should be approved")] string leagueName,
        [Description("Tag the Discord member to remove")] DiscordMember driver)
        {
            using (LoggingHelper.BeginScope(_logger, ctx))
            {
                _logger.LogDebug("Command: !league kick {leagueName} {driver} by {member}", leagueName, driver.DisplayName, ctx.Member.DisplayName);
                await ctx.TriggerTypingAsync();

                var response = await Mediator!.Send(new LeagueLeaveRequest
                {
                    GuildId = ctx.Guild.Id.ToString(),
                    LeagueName = leagueName,
                    DiscordMemberId = driver.Id.ToString()
                });

                var responseMessage = response
                    .ToResponseMessage($"{driver} removed the {leagueName} league.");

                await ctx.RespondAsync(responseMessage);
            }
        }

        [Command("approve")]
        [Description("Approve a driver to participate in the league, move from the waiting list to the driver list.")]
        [RequireRoles(RoleCheckMode.Any, ADMIN_ROLE)]
        public async Task ApproveParticipant(CommandContext ctx,
        [Description("The league where the driver should be approved")] string leagueName,
        [Description("Tag the Discord member to approve")] DiscordMember driver)
        {
            using (LoggingHelper.BeginScope(_logger, ctx))
            {
                _logger.LogDebug("Command: !league approve {leagueName} by {member}", leagueName, ctx.Member.DisplayName);
                await ctx.TriggerTypingAsync();

                var response = await Mediator!.Send(new LeagueParticipantApproalRequest
                {
                    GuildId = ctx.Guild.Id.ToString(),
                    LeagueName = leagueName,
                    ApprovedBy = ctx.Member.Id.ToString(),
                    DiscordMemberId = driver.Id.ToString()
                });

                var responseMessage = response
                    .ToResponseMessage($"{driver} approved to drive in the {leagueName} league.");

                await ctx.RespondAsync(responseMessage);
                await driver.SendMessageAsync($"You were approved for the {leagueName} league, see you on track soon!");
            }
        }

        [Command("remove")]
        [Description("Deletes the league")]
        public async Task RemoveLeague(CommandContext ctx, string leagueName)
        {
            using (LoggingHelper.BeginScope(_logger, ctx))
            {
                _logger.LogDebug("Command: !league remove {leagueName} by {member}", leagueName, ctx.Member.DisplayName);
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
        }

        [Command("status")]
        [Description("Shows the status for the league")]
        public async Task LeagueStatus(CommandContext ctx, string leagueName)
        {
            using (LoggingHelper.BeginScope(_logger, ctx))
            {
                _logger.LogDebug("Command: !league status {leagueName} by {member}", leagueName, ctx.Member.DisplayName);
                await ctx.TriggerTypingAsync();

                var response = await Mediator!.Send(new LeagueQueryRequest
                {
                    GuildId = ctx.Guild.Id.ToString(),
                    Query = (l) => l.Name.ToLowerInvariant() == leagueName.ToLowerInvariant()
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

                var waitingListParticipants = league?.Participants.Count(p => p.Value.Status == LeagueParticipantStatus.Pending);
                var activeParticipants = league?.Participants.Count(p => p.Value.Status == LeagueParticipantStatus.Approved);

                var builder = new DiscordEmbedBuilder()
                        .WithTitle(league?.Name)
                        .WithUrl(league?.Standings)
                        .WithDescription(league?.Description)
                        .WithColor(DiscordColor.DarkRed)
                        .AddField("Status", league?.Status.ToString())
                        .AddField("Drivers", activeParticipants.ToString(), true)
                        .AddField("Max", league?.MaxGrid.HasValue == true ? league?.MaxGrid.ToString() : "n/a", true)
                        .AddField("Waiting List", waitingListParticipants.ToString(), true)
                        .AddField("Created On", league?.CreatedDate.ToShortDateString());

                await ctx.Channel.SendMessageAsync(builder);
            }
        }

        [Command("open")]
        [Description("Set the league to open for automatic approval")]
        public async Task SetLeagueOpen(CommandContext ctx,
            string leagueName,
            [Description("Maximum grid positions, also the maximum number of automatic approvals before the league is closed")] int gridSpots)
        {
            using (LoggingHelper.BeginScope(_logger, ctx))
            {
                _logger.LogDebug("Command: !league open {leagueName} by {member}", leagueName, ctx.Member.DisplayName);
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

                await ctx.RespondAsync($"League **{leagueName}** is now open with auto approval up to **{gridSpots}**.");
            }
        }

        [Command("close")]
        [Description("Set the league to closed for automatic approval")]
        public async Task SetLeagueClosed(CommandContext ctx, string leagueName)
        {
            using (LoggingHelper.BeginScope(_logger, ctx))
            {
                _logger.LogDebug("Command: !league close {leagueName} by {member}", leagueName, ctx.Member.DisplayName);
                await ctx.TriggerTypingAsync();

                var response = await Mediator!.Send(new LeagueCloseRequest
                {
                    GuildId = ctx.Guild.Id.ToString(),
                    LeagueName = leagueName,
                });

                if (response.HasErrors())
                {
                    await ctx.RespondAsync(response.ToErrorMessage());
                    return;
                }

                await ctx.RespondAsync($"League **{leagueName}** is now closed and all new registrations will be put on the waiting list.");
            }
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
            using (LoggingHelper.BeginScope(_logger, ctx))
            {
                _logger.LogDebug("Command: !league participants {leagueName} by {member}", leagueName, ctx.Member.DisplayName);
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

                var drivers = league?.Participants.Where(p => filterExpression(p.Value)).Select(p => p.Value);

                if (drivers?.Any() == false)
                {
                    await ctx.RespondAsync(":shrug: No drivers found for your query");
                    return;
                }

                var fileName = $"drivers_{Guid.NewGuid()}.txt";

                using var stream = new MemoryStream();
                using var writer = new StreamWriter(stream);
                writer.WriteLine("DiscordMemberId | DiscordMemberUser | IRacingName | IRacingCustomerId | RegistrationDate | Status");

                foreach (var d in drivers ?? Enumerable.Empty<LeagueParticipantModel>())
                {
                    var driverText = $"{d.DiscordMemberId} | {d.DicordMemberUser} | {d.IRacingName} | {d.IRacingCustomerId} | {d.RegistrationDate} | {d.Status}";
                    writer.WriteLine(driverText);
                }

                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                var msg = new DiscordMessageBuilder()
                    .WithContent($"Drivers for {leagueName}")
                    .WithFile($"{leagueName}_participants.csv", stream);

                await ctx.Channel.SendMessageAsync(msg);
            }
        }

        [GroupCommand]
        [Command("list")]
        [Description("Shows all the leagues")]
        public async Task ListLeagues(CommandContext ctx)
        {
            using (LoggingHelper.BeginScope(_logger, ctx))
            {
                _logger.LogDebug("Command: !league list by {member}", ctx.Member.DisplayName);
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
                        x.MaxGrid,
                        x.Status,
                        CreatedOn = x.CreatedDate,
                        Pending = x.Participants.Count(p => p.Value.Status == LeagueParticipantStatus.Pending),
                        Approved = x.Participants.Count(p => p.Value.Status == LeagueParticipantStatus.Approved),
                        x.Standings
                    });

                foreach (var league in leagues)
                {
                    var builder = new DiscordEmbedBuilder()
                        .WithTitle(league.Name)
                        .WithUrl(league.Standings)
                        .WithDescription(league.Description)
                        .WithColor(DiscordColor.DarkRed)
                        .AddField("Status", league.Status.ToString())
                        .AddField("Drivers", league.Approved.ToString(), true)
                        .AddField("Max", league.MaxGrid.HasValue ? league.MaxGrid.ToString() : "n/a", true)
                        .AddField("Waiting List", league.Pending.ToString(), true)
                        .AddField("Created On", league.CreatedOn.ToShortDateString());

                    await ctx.Channel.SendMessageAsync(builder);
                }
            }
        }

        private async Task<LeagueJoinRequest?> BuildLeagueJoinRequest(CommandContext ctx, string leagueName)
        {
            var interactivity = ctx.Client.GetInteractivity();
            await ctx.Member.SendMessageAsync("We need a bit more information ");

            var msg = await ctx.Member.SendMessageAsync("What is your name? (As used in iRacing.)");

            var response = await interactivity.WaitForMessageAsync((m) => m.ChannelId == msg.ChannelId && m.Author == ctx.Member);
            if (await RespondIfTimeout(ctx, response)) return null;

            var name = response.Result.Content;

            msg = await ctx.Member.SendMessageAsync("What is your iRacing Customer ID? (Optional, respond with `none` fi you don't know)");
            response = await interactivity.WaitForMessageAsync((m) => m.ChannelId == msg.ChannelId && m.Author == ctx.Member); ;
            if (await RespondIfTimeout(ctx, response)) return null;

            if (!int.TryParse(response.Result.Content, out int customerId))
            {
                customerId = -1;
            }

            msg = await ctx.Member.SendMessageAsync("Have you read and agree with the Snail Racing code of conduct `yes/no` (https://annieandlarry.com/snail-racing-conduct)");
            response = await interactivity.WaitForMessageAsync((m) => m.ChannelId == msg.ChannelId && m.Author == ctx.Member);
            if (await RespondIfTimeout(ctx, response)) return null;

            var agreeTermsAndConditions = response.Result.Content.ToLower() == "yes";

            var request = new LeagueJoinRequest
            {
                GuildId = ctx.Guild.Id.ToString(),
                DiscordMemberId = ctx.Member.Id.ToString(),
                DiscordMemberUser = ctx.Member.Username,
                LeagueName = leagueName,
                IRacingName = name,
                IRacingCustomerId = customerId,
                AgreeTermsAndConditions = agreeTermsAndConditions
            };

            return request;
        }

        private async Task<bool> RespondIfTimeout(CommandContext ctx, DSharpPlus.Interactivity.InteractivityResult<DiscordMessage> response)
        {
            if (response.TimedOut)
            {
                _logger.LogWarning("Command: !league join timeout on questions from {member} ", ctx.Member.DisplayName);
                await ctx.Member.SendMessageAsync("Sorry, I am a little impatient and you took too long to respond, please start again to join the league");
            }

            return response.TimedOut;
        }
    }
}
