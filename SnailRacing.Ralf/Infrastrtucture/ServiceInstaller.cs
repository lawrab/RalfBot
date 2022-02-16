using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SnailRacing.Ralf.Logging;

namespace SnailRacing.Ralf.Infrastrtucture
{
    public static class ServiceInstaller
    {
        public static async Task<ServiceProvider> ConfigureServices(AppConfig appConfig, DiscordSink discordSink)
        {
            return new ServiceCollection()
                    .AddLogging(l => l.AddSerilog())
                    .AddSingleton(appConfig)
                    .AddSingleton(await StorageHelper.CreateRoleStorage(Path.Combine(appConfig?.DataPath ?? string.Empty, "roleStorage.json")))
                    .AddSingleton(await StorageHelper.CreateNewsStorage(Path.Combine(appConfig?.DataPath ?? string.Empty, "newsStorage.json")))
                    .AddSingleton(await StorageHelper.CreateLeaguesStorage(Path.Combine(appConfig?.DataPath ?? string.Empty, "leaguesStorage.json")))
                    .AddSingleton(discordSink)
                    .BuildServiceProvider();
        }
    }
}
