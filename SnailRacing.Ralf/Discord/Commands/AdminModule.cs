﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using SnailRacing.Ralf.Logging;

namespace SnailRacing.Ralf.Discord.Commands
{
    [Group("admin")] // let's mark this class as a command group
    [Description("Administrative commands.")] // give it a description for help purposes
    [Hidden] // let's hide this from the eyes of curious users
    [RequirePermissions(Permissions.ManageGuild)] // and restrict this to users who have appropriate permissions
    internal class AdminModule : BaseCommandModule
    {
        [Group("tail"), Hidden]
        public class LoggingModule : BaseCommandModule
        {
            private readonly DiscordSink _discordSink;
            private readonly ILogger<LoggingModule> _logger;

            public LoggingModule(DiscordSink discordSink, ILogger<LoggingModule> logger)
            {
                _discordSink = discordSink;
                _logger = logger;
            }

            [GroupCommand]
            public async Task ShowLoggingChannel(CommandContext ctx)
            {
                await ctx.TriggerTypingAsync();

                var loggingChannel = _discordSink.GetChannel()?.Name ?? "Not Set";

                await ctx.RespondAsync($"Tailing log in {loggingChannel}");
            }

            [Command("on")]
            public async Task TailOn(CommandContext ctx, DiscordChannel channel)
            {
                await ctx.TriggerTypingAsync();

                _discordSink.SetChannel(channel);
                _discordSink.Enable();

                _logger.LogInformation("Turned tail on in {channel}", channel);
                await ctx.RespondAsync($"Logging channel set to `{channel}`");
            }

            [Command("off")]
            public async Task TailOff(CommandContext ctx)
            {
                await ctx.TriggerTypingAsync();

                _logger.LogInformation("Turning tail off in {channel}", _discordSink.Channel);
                _discordSink.Disable();

                await ctx.RespondAsync($"Tailing off in {_discordSink?.Channel?.Mention ?? "unknown"}");
            }

            [Command("ping")]
            public async Task Ping(CommandContext ctx)
            {
                await ctx.TriggerTypingAsync();

                var msg = $"Yes, yes, I am here with a {ctx.Client.Ping}ms delay in reactions time.";
                await ctx.RespondAsync(msg);
            }
        }
    }
}
