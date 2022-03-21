using DSharpPlus.Entities;
using Serilog.Core;
using Serilog.Events;
using System.Collections.Concurrent;

namespace SnailRacing.Ralf.Logging
{
    public class DiscordSink : ILogEventSink
    {
        private bool _enabled = false;
        private readonly ConcurrentDictionary<string, DiscordChannel> _channels = new();
        private IFormatProvider? _formatProvider;

        public ConcurrentDictionary<string, DiscordChannel> Channels
        {
            get => _channels;

            private set { }
        }

        public void Emit(LogEvent logEvent)
        {
            var guildId = logEvent.Properties.ContainsKey("GuildId") ? logEvent.Properties["GuildId"].ToString() : null;

            if(_enabled && !string.IsNullOrEmpty(guildId) && _channels.ContainsKey(guildId))
            {
                var channel = _channels[guildId];
                if (logEvent.Level >= LogEventLevel.Information)
                {
                    var embed = BuildMessage(logEvent);
                    channel.SendMessageAsync(embed).ConfigureAwait(false);
                }
            }
        }

        public void SetFormatProvider(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
        }

        public void Enable()
        {
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
        }

        public void AddChannel(DiscordChannel channel)
        {
            _channels.TryAdd(channel.Guild.Id.ToString(), channel);
        }

        public DiscordChannel? GetChannel(string guildId)
        {
            return _channels[guildId];
        }

        public void RemoveChannel(string guildId)
        {
            _channels.TryRemove(guildId, out _);
        }

        private DiscordEmbed BuildMessage(LogEvent logEvent)
        {
            return new DiscordEmbedBuilder()
                .WithTitle(logEvent.Level.ToString())
                .WithDescription(logEvent.RenderMessage(_formatProvider))
                .WithEmbedLogLevel(logEvent.Level)
                .Build();
        }
    }
}
