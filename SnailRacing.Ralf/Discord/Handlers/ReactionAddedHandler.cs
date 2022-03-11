using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Discord.Handlers
{
    public class ReactionAddedHandler
    {
        private readonly IStorageProvider _storageProvider;
        private readonly ILogger<ReactionAddedHandler> _logger;

        public ReactionAddedHandler(IStorageProvider storageProvider, ILogger<ReactionAddedHandler> logger)
        {
            _storageProvider = storageProvider;
            _logger = logger;
        }

        public Task HandleReactionAdded(DiscordClient client, MessageReactionAddEventArgs e)
        {
            var newsPaperEmoji = DiscordEmoji.FromName(client, ":newspaper:");

            if(e.Emoji == newsPaperEmoji)
            {
                _logger.LogInformation("Newspaper reaction added to message ({messageId})", e.Message.Id);
                AddNews(e);
            }

            return Task.CompletedTask;
        }

        // ToDo: move this to mediatr
        private void AddNews(MessageReactionAddEventArgs eventArgs)
        {
            var store = StoreHelper.GetNewsStore(eventArgs.Guild.Id.ToString(), _storageProvider);
            var key = eventArgs.Message.Id.ToString();

            store.TryAdd(key, new NewsModel
            {
                Who = eventArgs.Message.Author?.Username ?? "Unknown",
                Story = eventArgs.Message.Content ?? "No content",
                When = eventArgs.Message.CreationTimestamp.UtcDateTime
            });
        }
    }
}
