using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Logging;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Discord.Commands
{
    [Group("admin")] // let's mark this class as a command group
    [Description("Administrative commands.")] // give it a description for help purposes
    [Hidden] // let's hide this from the eyes of curious users
    [RequireRoles(RoleCheckMode.Any, ADMIN_ROLE)] // and restrict this to users who have appropriate permissions
    internal class AdminModule : BaseCommandModule
    {
        private const string ADMIN_ROLE = "Ralf Admin";
        private readonly ILogger<AdminModule> _logger;

        public AdminModule(ILogger<AdminModule> logger)
        {
            _logger = logger;
        }

        [Command("ping")]
        public async Task Ping(CommandContext ctx)
        {
            using (LoggingHelper.BeginScope(_logger, ctx))
            {
                await ctx.TriggerTypingAsync();

                var msg = $"Yes, yes, I am here with a `{ctx.Client.Ping}ms` delay in reaction time.";
                await ctx.RespondAsync(msg);
            }
        }

        [Group("tail"), Hidden]
        public class LoggingModule : BaseCommandModule
        {
            private readonly DiscordSink _discordSink;
            private readonly ILogger<LoggingModule> _logger;
            private readonly IStorageProvider _storageProvider;

            public LoggingModule(DiscordSink discordSink, ILogger<LoggingModule> logger, IStorageProvider storageProvider)
            {
                _discordSink = discordSink;
                _logger = logger;
                _storageProvider = storageProvider;
            }

            [GroupCommand]
            public async Task ShowLoggingChannel(CommandContext ctx)
            {
                using (LoggingHelper.BeginScope(_logger, ctx))
                {

                    await ctx.TriggerTypingAsync();

                    var loggingChannel = _discordSink.GetChannel(ctx.Guild.Id.ToString())?.Name ?? "Not Set";

                    await ctx.RespondAsync($"Tailing log in {loggingChannel}");
                }
            }

            [Command("on")]
            public async Task TailOn(CommandContext ctx, DiscordChannel channel)
            {
                using (LoggingHelper.BeginScope(_logger, ctx))
                {

                    await ctx.TriggerTypingAsync();

                    SaveLogging(ctx, channel, true);

                    _discordSink.AddChannel(channel);
                    _discordSink.Enable();

                    _logger.LogInformation("Turned tail on in {channel}", channel);
                    await ctx.RespondAsync($"Logging channel set to `{channel}`");
                }
            }

            [Command("off")]
            public async Task TailOff(CommandContext ctx)
            {
                using (LoggingHelper.BeginScope(_logger, ctx))
                {
                    await ctx.TriggerTypingAsync();
                    var guildId = ctx.Guild.Id.ToString();
                    var channel = _discordSink.GetChannel(guildId);
                    _logger.LogInformation("Turning tail off in {channel}", channel);

                    _discordSink.RemoveChannel(guildId);

                    SaveLogging(ctx, null, false);

                    _discordSink.Disable();

                    await ctx.RespondAsync($"Tailing off in {channel?.Mention ?? "unknown"}");
                }
            }
            private void SaveLogging(CommandContext ctx, DiscordChannel? channel, bool isTailOn)
            {
                var guildId = ctx.Guild.Id.ToString();

                var guildConfigStore = StoreHelper.GetGuildConfigStore(guildId, _storageProvider);
                var guildConfig = guildConfigStore[guildId];
                guildConfig.IsTailOn = isTailOn;
                guildConfig.LoggingChannelId = channel?.Id;
                guildConfigStore.TryUpdate(guildId, guildConfig);
            }
        }
    }
}
