using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.DiscordCommands
{
    [Group("admin")] // let's mark this class as a command group
    [Description("Administrative commands.")] // give it a description for help purposes
    [Hidden] // let's hide this from the eyes of curious users
    [RequirePermissions(Permissions.ManageGuild)] // and restrict this to users who have appropriate permissions
    internal class AdminModule : BaseCommandModule
    {
        public IStorageProvider<string, object>? StorageProvider { private get; set; }

        [Group("log"), Hidden]
        public class LoggingModule : BaseCommandModule
        {
            public IStorageProvider<string, object>? StorageProvider { private get; set; }

            [GroupCommand]
            public async Task ShowLoggingChannel(CommandContext ctx)
            {
                await ctx.TriggerTypingAsync();

                var loggingChannel = this.StorageProvider![StorageProviderKeys.LOGGING_CHANNEL];

                await ctx.RespondAsync($"Logging channel is {loggingChannel}");
            }

            [Command("set")]
            public async Task SetLoggingChannel(CommandContext ctx, DiscordChannel channel)
            {
                await ctx.TriggerTypingAsync();

                StorageProvider![StorageProviderKeys.LOGGING_CHANNEL] = channel;

                await ctx.RespondAsync($"Logging channel set to `{channel}`");
            }
        }
    }
}
