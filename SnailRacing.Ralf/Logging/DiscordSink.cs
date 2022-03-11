using DSharpPlus.Entities;
using Serilog.Core;
using Serilog.Events;

namespace SnailRacing.Ralf.Logging
{
    public class DiscordSink : ILogEventSink
    {
        private bool _enabled = false;
        private DiscordChannel? _channel;
        private IFormatProvider? _formatProvider;

        public DiscordChannel? Channel
        {
            get => _channel;

            private set { }
        }

        public void Emit(LogEvent logEvent)
        {
            if (!_enabled || _channel == null) return;

            var embed = BuildMessage(logEvent);
            _channel.SendMessageAsync(embed);
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

        public void SetChannel(DiscordChannel channel)
        {
            _channel = channel;
        }

        public DiscordChannel? GetChannel()
        {
            return _channel;
        }

        public void ClearChannel()
        {
            _channel = null;
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
