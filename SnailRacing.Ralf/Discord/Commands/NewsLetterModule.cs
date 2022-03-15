using CoreHtmlToImage;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MediatR;
using SnailRacing.Ralf.Handlers.News;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Discord.Commands
{
    [Group("news")]
    public class NewsLetterModule : BaseCommandModule
    {
        private readonly IStorageProvider _storageProvider;
        
        public IMediator? Mediator { get; set; }

        public NewsLetterModule(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        [GroupCommand]
        [Command("list")]
        public async Task ListNews(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var response = await Mediator!.Send(new NewsQueryRequest
            {
                GuildId = ctx.Guild.Id.ToString()
            });

            var newsItems = response.News
                .Select(n => $"<div><h3>{n.Who} - {n.When.ToShortDateString()}</h3><p>{n.Story}</p></div>")
                .ToList();

            var htmlString = newsItems.Any() ? string.Concat(newsItems, Environment.NewLine) : "<div><h3>No news is good news<h3></div>";

            var convertor = new HtmlConverter();
            var bytes = convertor.FromHtmlString(htmlString);

            using var stream = new MemoryStream(bytes);

            var builder = new DiscordMessageBuilder()
                .WithFile("news.jpg", stream);

            await ctx.RespondAsync(builder);
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
