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
        private readonly IMediator _mediator;
        private readonly IStorageProvider _storageProvider;

        public NewsLetterModule(IMediator mediator, IStorageProvider storageProvider)
        {
            _mediator = mediator;
            _storageProvider = storageProvider;
        }

        [GroupCommand]
        [Command("show")]
        public async Task ListNews(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var response = await _mediator.Send(new NewsQueryRequest
            {
                GuildId = ctx.Guild.Id.ToString(),
                Filter = m => m.When.AddDays(31) >= DateTime.UtcNow
            });

            var newsItems = response.News
                .Select(n => $"<div><h3>{n.Who} - {n.When.ToShortDateString()}</h3><p>{n.Story}</p></div>")
                .ToList();

            var htmlString = newsItems.Any() ? string.Join(Environment.NewLine, newsItems) : "<div><h3>No news is good news<h3></div>";

            var bytes = GeneratePdf(htmlString);

            using var stream = new MemoryStream(bytes);

            var builder = new DiscordMessageBuilder()
                .WithFile("news.png", stream);

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
        private byte[] GeneratePdf(string htmlContent)
        {
            var convertor = new HtmlConverter();

            return convertor.FromHtmlString(htmlContent, 1024, CoreHtmlToImage.ImageFormat.Png, 100);
        }
    }
}
