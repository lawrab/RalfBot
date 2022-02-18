using Microsoft.Extensions.Configuration;

namespace SnailRacing.Ralf.Models
{
    public class AppConfig
    {
        public DiscordConfig Discord { get; } = new DiscordConfig();
        public string? DataPath { get; set; } = String.Empty;

        public static AppConfig Create()
        {
            var _config = InitConfig();
            var appConfig = new AppConfig();
            _config.Bind(appConfig);

            return appConfig;
        }

        private static IConfigurationRoot InitConfig()
        {
            var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables();

            if (env?.ToUpper() == "DEVELOPMENT") builder.AddUserSecrets<Program>();

            return builder.Build();
        }
    }
}
