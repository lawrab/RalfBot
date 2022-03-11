using Serilog;
using Serilog.Configuration;

namespace SnailRacing.Ralf.Logging
{
    public static class DiscordSinkExtension
    {
        public static LoggerConfiguration DiscordSink(
                  this LoggerSinkConfiguration loggerConfiguration,
                  DiscordSink sink,
                  IFormatProvider? formatProvider = null)
        {
            if(formatProvider != null) sink.SetFormatProvider(formatProvider);
            return loggerConfiguration.Sink(sink);
        }
    }
}
