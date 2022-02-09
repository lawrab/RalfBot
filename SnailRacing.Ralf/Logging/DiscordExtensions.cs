using DSharpPlus.Entities;
using Serilog.Events;

namespace SnailRacing.Ralf.Logging
{
    public static class DiscordExtensions
    {
        public static DiscordEmbedBuilder WithEmbedLogLevel(this DiscordEmbedBuilder embedBuilder, LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Verbose:
                    embedBuilder.Title = ":loud_sound: Verbose";
                    embedBuilder.Color = DiscordColor.LightGray;
                    break;
                case LogEventLevel.Debug:
                    embedBuilder.Title = ":mag: Debug";
                    embedBuilder.Color = DiscordColor.LightGray;
                    break;
                case LogEventLevel.Information:
                    embedBuilder.Title = ":information_source: Information";
                    embedBuilder.Color = new DiscordColor(0, 186, 255);
                    break;
                case LogEventLevel.Warning:
                    embedBuilder.Title = ":warning: Warning";
                    embedBuilder.Color = new DiscordColor(255, 204, 0);
                    break;
                case LogEventLevel.Error:
                    embedBuilder.Title = ":x: Error";
                    embedBuilder.Color = new DiscordColor(255, 0, 0);
                    break;
                case LogEventLevel.Fatal:
                    embedBuilder.Title = ":skull_crossbones: Fatal";
                    embedBuilder.Color = DiscordColor.DarkRed;
                    break;
                default:
                    break;
            }

            return embedBuilder;
        }
    }
}
