using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.DiscordCommands
{
    [Group("news")]
    public class NewsLetterModule : BaseCommandModule
    {
        public IStorageProvider<NewsStorageProviderModel>? StorageProvider { private get; set; }

        [GroupCommand]
        public async Task AddNews(CommandContext ctx, [RemainingText] string newsMessage)
        {
            await ctx.TriggerTypingAsync();
            StorageProvider!.Store.Add(new NewsModel
            {
                Who = ctx.Member.DisplayName,
                Story = newsMessage
            });
            var emoji = DiscordEmoji.FromName(ctx.Client, ":newspaper:");

            await ctx.RespondAsync($"{emoji} added, stay awesome!");
        }


        [Command("list")]
        public async Task ListNews(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var news = StorageProvider!.Store.Query(10);

            var emoji = DiscordEmoji.FromName(ctx.Client, ":newspaper2:");

            var embed = new DiscordEmbedBuilder
            {
                Title = $"{emoji} Latest News"
            };
            news?.ToList().ForEach(i => embed.AddField(i.Who, i.Story, true));

            await ctx.RespondAsync(embed);
        }

        [Command("csv")]
        public async Task ToCSV(CommandContext ctx)
        {
            await ToCSV(ctx, DateTime.UtcNow);  
        }

        [Command("csv")]
        [RequireDirectMessage()]
        public async Task ToCSV(CommandContext ctx, DateTime date)
        {
            await ctx.TriggerTypingAsync();

            var news = StorageProvider!.Store.QueryMonth(date);

            var emoji = DiscordEmoji.FromName(ctx.Client, ":newspaper2:");

            var csv = news?.Select(n => $"{n.Who}|{n.When}|{n.Story}") ?? Enumerable.Empty<string>();

            var builder = new DiscordMessageBuilder();
            builder.WithContent($"{emoji} Newsletter (${DateTime.UtcNow.ToString("MMM, yyyy")})")
                .WithContent($"`{string.Join(Environment.NewLine, csv)}`");

            await ctx.RespondAsync(builder);
        }
    }
}
