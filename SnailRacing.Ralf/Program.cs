global using DSharpPlus;
global using DSharpPlus.EventArgs;
global using SnailRacing.Ralf.Models;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SnailRacing.Ralf.Discord.Commands;
using SnailRacing.Ralf.Discord.Handlers;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Logging;
using SnailRacing.Ralf.Providers;

MainAsync().GetAwaiter().GetResult();

static async Task MainAsync()
{
    var appConfig = AppConfig.Create();
    var discordSink = new DiscordSink();
    ConfigureLogging(discordSink);
    var loggerFactory = new LoggerFactory().AddSerilog();
    var services = await ServiceInstaller.ConfigureServices(appConfig, discordSink);
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

    // ToDo: tidy up and use a assebly registration instead, so all modules in the assembly is always registered
    commands.RegisterCommands<DiscordRolesModule>();
    commands.RegisterCommands<AdminModule>();
    commands.RegisterCommands<NewsLetterModule>();
    commands.RegisterCommands<LeagueModule>();

    discord.GuildMemberUpdated += async (s, e) =>
    {
        var storage = services.GetService<IStorageProvider<RolesStorageProviderModel>>();
        var handler = new RoleChangedHandler(storage);

        await handler.HandleRoleChange(e);
        e.Handled = true;
    };

    await discord.ConnectAsync();

    return discord;
}