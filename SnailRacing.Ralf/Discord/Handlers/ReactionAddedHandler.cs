using DSharpPlus.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using SnailRacing.Ralf.Handlers.News;
using SnailRacing.Ralf.Logging;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Discord.Handlers
{
    public class ReactionAddedHandler
    {
        private readonly IMediator _mediator;
        private readonly IStorageProvider _storageProvider;
        private readonly ILogger<ReactionAddedHandler> _logger;

        public ReactionAddedHandler(IMediator mediator, IStorageProvider storageProvider, ILogger<ReactionAddedHandler> logger)
        {
            _mediator = mediator;
            _storageProvider = storageProvider;
            _logger = logger;
        }

        public async Task HandleReactionAdded(DiscordClient client, MessageReactionAddEventArgs e)
        {
            using (LoggingHelper.BeginScope(_logger, e.Guild.Id, e.User.Username))
            {
                var newsPaperEmoji = DiscordEmoji.FromName(client, ":newspaper:");

                if (e.Emoji == newsPaperEmoji)
                {
                    _logger.LogInformation("Newspaper reaction added to message ({messageId})", e.Message.Id);

                    var response = await _mediator.Send(new NewsAddRequest
                    {
                        GuildId = e.Guild.Id.ToString(),
                        Who = e.Message.Author?.Username ?? "Unknown",
                        When = e.Message.CreationTimestamp.UtcDateTime,
                        Story = e.Message.Content
                    });
                }
            }
        }
    }
}
