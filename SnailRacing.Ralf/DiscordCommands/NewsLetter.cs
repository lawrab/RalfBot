using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.DiscordCommands
{
    [Group("news")]
    public class NewsLetter : BaseCommandModule
    {
        public IStorageProvider<NewsStorageProviderModel>? StorageProvider { private get; set; }

        [GroupCommand]
        public async Task AddNews(CommandContext ctx, DiscordMember member, [RemainingText] string newsMessage)
        {
            await ctx.TriggerTypingAsync();
            StorageProvider!.Store.Add(new NewsModel
            {
                Who = member.Nickname,
                Story = newsMessage
            });
            var emoji = DiscordEmoji.FromName(ctx.Client, ":newspaper2:");

            await ctx.RespondAsync($"{emoji} added, stay awesome!");
        }
    }
}
