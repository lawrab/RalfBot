using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;

namespace SnailRacing.Ralf.Logging
{
    public static class LoggingHelper
    {
        public static IDisposable BeginScope(ILogger logger, CommandContext ctx)
        {
            var props = new Dictionary<string, object>
            {
                {"GuildId", ctx.Guild.Id },
                {"UserName", ctx.Member.Username },
                {"Message", ctx.Message }
            };

            return logger.BeginScope(props);
        }

        public static IDisposable BeginScope(ILogger logger, ulong guildId, string username)
        {
            var props = new Dictionary<string, object>
            {
                {"GuildId", guildId },
                {"UserName", username },
            };

            return logger.BeginScope(props);
        }
    }
}
