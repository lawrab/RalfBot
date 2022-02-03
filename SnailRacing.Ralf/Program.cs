global using DSharpPlus;
global using DSharpPlus.EventArgs;
global using SnailRacing.Ralf;
global using SnailRacing.Ralf.Models;

MainAsync().GetAwaiter().GetResult();

static async Task MainAsync()
{
    var discord = new DiscordClient(new DiscordConfiguration()
    {
        Token = AppConfig.Discord.BotToken,
        TokenType = TokenType.Bot,
        Intents = DiscordIntents.AllUnprivileged | DiscordIntents.GuildMembers
    });
    
    discord.MessageCreated += async (s, e) =>
    {
        if (e.Message.Content.ToLower().StartsWith("ping"))
            await e.Message.RespondAsync("pong!");
    };

    discord.GuildMemberUpdated += async (s, e) =>
    {
        //var channel = await discord.GetChannelAsync(935530785006551082);
        //await discord.SendMessageAsync(channel, "UserUpdated");

        await RoleChangedHandler.UpdateRoles(discord, e);
    };

    discord.GuildRoleUpdated += async (s, e) =>
    {
        var channel = await discord.GetChannelAsync(935530785006551082);
        //await discord.SendMessageAsync(channel, "GuildRoleUpdated");
    };

    await discord.ConnectAsync();

    await Task.Delay(-1);
}





