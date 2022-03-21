using Microsoft.Extensions.DependencyInjection;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Logging;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Discord.Handlers
{
    public static class GuildEventHandlers
    {
        public static async Task GuidAvailableHandler(DiscordClient client, GuildCreateEventArgs eventArgs, IServiceProvider services)
        {
            // ToDo: needs to be improved and moved to mediatr
            var storageProvider = services.GetService<IStorageProvider>();
            var guildConfig = StoreHelper.GetGuildConfig(eventArgs.Guild.Id.ToString(), storageProvider);

            // ToDo: move to configuration and settings
            // make it guild specific, this is currently global
            // Logging to SnailRacing ralf-log
            if (guildConfig.IsTailOn && guildConfig.LoggingChannelId.HasValue)
            {
                var loggingChannel = await client.GetChannelAsync(guildConfig.LoggingChannelId.GetValueOrDefault());
                var discordSink = services.GetService<DiscordSink>();
                discordSink?.AddChannel(loggingChannel);
                discordSink?.Enable();
            }
        }
    }
}
