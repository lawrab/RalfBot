global using DSharpPlus;
global using DSharpPlus.EventArgs;
global using SnailRacing.Ralf.Models;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SnailRacing.Ralf.DiscordCommands;
using SnailRacing.Ralf.Handlers;
using SnailRacing.Ralf.Logging;
using SnailRacing.Ralf.Providers;

MainAsync().GetAwaiter().GetResult();

static async Task MainAsync()
{
    var appConfig = AppConfig.Create();
    var discordSink = new DiscordSink();
    ConfigureLogging(discordSink);
    var loggerFactory = new LoggerFactory().AddSerilog();
    var services = await ConfigureServices(appConfig, discordSink);
    var discord = await ConnectToDiscord(services, loggerFactory, appConfig.Discord.BotToken);

    await Task.Delay(-1);
}

static void ConfigureLogging(DiscordSink discordSink) => Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.DiscordSink(discordSink)
    .CreateLogger();

static async Task<DiscordClient> ConnectToDiscord(ServiceProvider services, ILoggerFactory loggerFactory, string? token)
{
    var discord = new DiscordClient(new DiscordConfiguration()
    {
        LoggerFactory = loggerFactory, // ToDo: see if we can use ServiceProvider here instead, do we need to?
        Token = token,
        TokenType = TokenType.Bot,
        Intents = DiscordIntents.AllUnprivileged | DiscordIntents.GuildMembers
    });

    var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
    {
        StringPrefixes = new[] { "!" },
        Services = services
    });

    commands.RegisterCommands<DiscordRolesModule>();
    commands.RegisterCommands<AdminModule>();

    discord.GuildMemberUpdated += async (s, e) =>
    {
        var storage = services.GetService<IStorageProvider<string, object>>();
        var handler = new RoleChangedHandler(storage);

        await handler.HandleRoleChange(e);
        e.Handled = true;
    };

    await discord.ConnectAsync();

    return discord;
}

static async Task<ServiceProvider> ConfigureServices(AppConfig appConfig, DiscordSink discordSink)
{
    return new ServiceCollection()
            .AddLogging(l => l.AddSerilog())
            .AddSingleton(appConfig)
            .AddSingleton(await CreateStorage(appConfig.DataPath ?? "appData.json"))
            .AddSingleton(discordSink)
            .BuildServiceProvider();
}

static async Task<IStorageProvider<string, object>> CreateStorage(string dataPath)
{
    var fileStorageProvider = new JsonFileStorageProvider<StorageProviderModel<string, object>>(dataPath);
    var storageProvider = new StorageProvider<string, object>();
    await storageProvider.SetFileStorageProvider(fileStorageProvider);

    return storageProvider;
}