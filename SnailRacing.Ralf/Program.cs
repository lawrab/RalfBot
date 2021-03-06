global using DSharpPlus;
global using DSharpPlus.EventArgs;
global using SnailRacing.Ralf.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using MediatR;
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
    var services = ServiceInstaller.ConfigureServices(appConfig, discordSink);
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
        MinimumLogLevel = LogLevel.Trace,
        Token = token,
        TokenType = TokenType.Bot,
        Intents = DiscordIntents.AllUnprivileged | DiscordIntents.GuildMembers
    });

    discord.UseInteractivity();
    var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
    {
        StringPrefixes = new[] { "!" },
        Services = services
    });
    commands.CommandErrored += Commands_CommandErrored;

    // ToDo: tidy up and use a assebly registration instead, so all modules in the assembly is always registered
    commands.RegisterCommands<DiscordRolesModule>();
    commands.RegisterCommands<AdminModule>();
    commands.RegisterCommands<NewsLetterModule>();
    commands.RegisterCommands<LeagueModule>();

    discord.GuildAvailable += (s, e) => GuildEventHandlers.GuidAvailableHandler(s, e, services);

    discord.GuildMemberUpdated += async (s, e) =>
    {
        var storage = services.GetService<IStorageProvider>();
        var handler = new RoleChangedHandler(storage!);

        await handler.HandleRoleChange(e);
        e.Handled = true;
    };

    // ToDo: move to DI
    discord.MessageReactionAdded += (s, e) =>
    {
        var storage = services.GetService<IStorageProvider>();
        var logger = services.GetService<ILogger<ReactionAddedHandler>>();
        var mediatr = services.GetService<IMediator>();
        var handler = new ReactionAddedHandler(mediatr!, storage!, logger!);

        return handler.HandleReactionAdded(s, e);
    };

    discord.MessageCreated += (s, e) =>
    {
        var storage = services.GetService<IStorageProvider>();
        var mediatr = services.GetService<IMediator>();

        _ = Task.Run(async () =>
        {
            var handler = new MessageCreatedHandler(mediatr!, storage!);

            await handler.HandleMessage(s, e);
        });

        return Task.CompletedTask;
    };

    await discord.ConnectAsync();

    return discord;
}

static async Task Commands_CommandErrored(CommandsNextExtension sender, CommandErrorEventArgs e)
{
    if (e.Exception is CommandNotFoundException) return;

    // let's log the error details
    e.Context.Client.Logger.LogError($"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' but it errored: {e.Exception.GetType()}: {e.Exception?.ToString() ?? "<no message>"}", DateTime.Now);

    // let's check if the error is a result of lack
    // of required permissions
    if (e.Exception is ChecksFailedException ex)
    {
        // yes, the user lacks required permissions, 
        // let them know

        var emoji = DiscordEmoji.FromName(e.Context.Client, ":no_entry:");

        // let's wrap the response into an embed
        var embed = new DiscordEmbedBuilder
        {
            Title = "Access denied",
            Description = $"{emoji} You do not have the permissions required to execute this command.",
            Color = new DiscordColor(0xFF0000) // red
        };
        await e.Context.RespondAsync(embed);
    }
}