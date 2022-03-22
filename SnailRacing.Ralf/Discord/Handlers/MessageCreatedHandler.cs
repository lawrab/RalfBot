using DSharpPlus.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using SnailRacing.Ralf.Handlers.General;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Discord.Handlers
{
    public class MessageCreatedHandler
    {
        private IMediator _mediator;
        private readonly IStorageProvider _storageProvider;

        public MessageCreatedHandler(IMediator mediator, IStorageProvider storageProvider)
        {
            _mediator = mediator;
            _storageProvider = storageProvider;
        }

        public async Task HandleMessage(DiscordClient s, MessageCreateEventArgs e)
        {
            if (e.Author.IsCurrent) return;

            await SendFactMessage(s, e);
        }

        private async Task SendFactMessage(DiscordClient s, MessageCreateEventArgs e)
        {
            if (!IsFactsOn(e.Guild.Id.ToString()) ||!ShouldCreateFact()) return;

            var fact = await _mediator.Send(new FactRequest());

            if (!string.IsNullOrEmpty(fact.Content))
            {
                var factEmoji = DiscordEmoji.FromName(s, ":thinking:", true);
                await e.Message.RespondAsync($"{factEmoji} {fact.Content}");
            }
        }

        private bool IsFactsOn(string guildId)
        {
            var guildConfig = StoreHelper.GetGuildConfig(guildId, _storageProvider);

            return guildConfig.IsFactsOn;
        }

        private bool ShouldCreateFact()
        {
            Random rnd = new Random();
            var n = rnd.Next(20);

            return n == 11;
        }
    }
}
