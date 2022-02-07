global using DSharpPlus;
global using DSharpPlus.EventArgs;
global using SnailRacing.Ralf;
global using SnailRacing.Ralf.Models;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using SnailRacing.Ralf.DiscordCommands;
using SnailRacing.Ralf.Providers;
using System.Collections.Concurrent;
using System.Reflection;

MainAsync().GetAwaiter().GetResult();

static async Task MainAsync()
{
    var services = await ConfigureServices();
    var discord = await ConnectToDiscord(services);

    await Task.Delay(-1);
}

static async Task<DiscordClient> ConnectToDiscord(ServiceProvider services)
{
    var discord = new DiscordClient(new DiscordConfiguration()
    {
        Token = AppConfig.Discord.BotToken,
        TokenType = TokenType.Bot,
        Intents = DiscordIntents.AllUnprivileged | DiscordIntents.GuildMembers
    });

    var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
    {
        StringPrefixes = new[] { "!" },
        Services = services
    });

    commands.RegisterCommands<DiscordRolesModule>();

    ////discord.MessageCreated += async (s, e) =>
    ////{
    ////    if (e.Message.Content.ToLower().StartsWith("ping"))
    ////        await e.Message.RespondAsync("pong!");
    ////};

    ////discord.GuildMemberUpdated += async (s, e) =>
    ////{
    ////    //var channel = await discord.GetChannelAsync(935530785006551082);
    ////    //await discord.SendMessageAsync(channel, "UserUpdated");

    ////    //await RoleChangedHandler.UpdateRoles(discord, e);
    ////};

    ////discord.GuildRoleUpdated += async (s, e) =>
    ////{
    ////    var channel = await discord.GetChannelAsync(935530785006551082);
    ////    //await discord.SendMessageAsync(channel, "GuildRoleUpdated");
    ////};

    await discord.ConnectAsync();

    return discord;
}

static async Task<ServiceProvider> ConfigureServices()
{
    return new ServiceCollection()
            .AddSingleton<IStorageProvider<string, object>>(await CreateStorage(AppConfig.DataPath))
            .BuildServiceProvider();
}

static async Task<IStorageProvider<string, object>> CreateStorage(string dataPath)
{
    var fileStorageProvider  = new JsonFileStorageProvider<StorageProviderModel<string, object>>(dataPath);
    var storageProvider = new StorageProvider<string, object>();
    await storageProvider.SetFileStorageProvider(fileStorageProvider);

    return storageProvider;
}