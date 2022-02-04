﻿using Microsoft.Extensions.Configuration;

namespace SnailRacing.Ralf.Models
{
    public static class AppConfig
    {
        public static DiscordConfig Discord { get; } = new DiscordConfig();
        public static string DataPath { get; }

        static AppConfig()
        {
            var _config = InitConfig();
            _config.GetSection("Discord").Bind(Discord);
            DataPath = _config["DataPath"] ?? "appData.json";
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
