using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SnailRacing.Ralf.Handlers.League;
using SnailRacing.Ralf.Infrastrtucture.PipelineBehaviours;
using SnailRacing.Ralf.Logging;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Infrastrtucture
{
    public static class ServiceInstaller
    {
        public static ServiceProvider ConfigureServices(AppConfig appConfig, DiscordSink discordSink)
        {
            return new ServiceCollection()
                    .AddLogging(l => l.AddSerilog())
                    .AddSingleton(appConfig)
                    .AddSingleton(discordSink)
                    .AddTransient(CreateStorageProvider)
                    .AddTransient(typeof(IPipelineBehavior<,>), typeof(LogginBehavior<,>))
                    .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>))
                    .AddValidatorsFromAssembly(typeof(ServiceInstaller).Assembly)
                    .AddMediatR(typeof(LeagueJoinHandler).Assembly)
                    .BuildServiceProvider();
        }

        private static IStorageProvider CreateStorageProvider(IServiceProvider services)
        {
            var config = services.GetService<AppConfig>();
            var logger = services.GetService<ILogger<StorageProvider>>();
            return StorageProvider.Create(config?.DataPath ?? string.Empty, logger!);
        }
    }
}
