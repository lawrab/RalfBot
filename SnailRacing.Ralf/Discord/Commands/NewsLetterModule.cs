using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Discord.Commands
{
    [Group("news")]
    public class NewsLetterModule : BaseCommandModule
    {
        private readonly IStorageProvider _storageProvider;

        public NewsLetterModule(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        [GroupCommand]
        [Command("list")]
        public async Task ListNews(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var store = StoreHelper.GetNewsStore(ctx.Guild.Id.ToString(), _storageProvider);

            var news = store.Take(3);

            var emoji = DiscordEmoji.FromName(ctx.Client, ":newspaper2:");

            var embed = new DiscordEmbedBuilder()
                .WithTitle($"{emoji} Latest News");
            news?.ToList().ForEach(i => embed.AddField(i.Value.Who, i.Value.Story ?? "No content", true));

            await ctx.RespondAsync(embed);
        }

        [Command("csv")]
        public async Task ToCSV(CommandContext ctx)
        {
            await ToCSV(ctx, DateTime.UtcNow);
        }

        [Command("csv")]
        public async Task ToCSV(CommandContext ctx, DateTime date)
        {
            await ctx.TriggerTypingAsync();

            var store = StoreHelper.GetNewsStore(ctx.Guild.Id.ToString(), _storageProvider);
            var news = store
                .Where(n => n.Value.When.AddDays(31) > date)
                .Select(n => n.Value);

            var emoji = DiscordEmoji.FromName(ctx.Client, ":newspaper2:");

            var csv = news?.Select(n => $"{n.Who}|{n.When}|{n.Story}") ?? Enumerable.Empty<string>();

            var builder = new DiscordMessageBuilder();
            builder.WithContent($"{emoji} Newsletter (${DateTime.UtcNow.ToString("MMM, yyyy")})")
                .WithContent($"`{string.Join(Environment.NewLine, csv)}`");

            await ctx.RespondAsync(builder);
        }
    }
}
