using MediatR;
using Microsoft.Extensions.Logging;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.News
{
    public class NewsAddHandler : IRequestHandler<NewsAddRequest, NewsAddResponse>
    {
        private readonly IStorageProvider _storageProvider;
        private readonly ILogger<NewsAddHandler> _logger;

        public NewsAddHandler(IStorageProvider storageProvider, ILogger<NewsAddHandler> logger)
        {
            _storageProvider = storageProvider;
            _logger = logger;
        }

        public Task<NewsAddResponse> Handle(NewsAddRequest request, CancellationToken cancellationToken)
        {
            var store = StoreHelper.GetNewsStore(request.GuildId, _storageProvider);
            if (!store.TryAdd(request.Key, new NewsModel
            {
                Who = request.Who,
                When = request.When,
                Story = request.Story
            }))
            {
                _logger?.LogWarning("News not saved {@request}", request);
            }

            return Task.FromResult(new NewsAddResponse { Key = request.Key });
        }
    }
}
