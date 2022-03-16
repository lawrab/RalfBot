using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.News
{
    public class NewsAddHandler
    {
        private readonly IStorageProvider _storageProvider;

        public NewsAddHandler(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public Task<NewsAddResponse> Handle(NewsAddRequest request, CancellationToken cancellationToken)
        {
            var store = StoreHelper.GetNewsStore(request.GuildId, _storageProvider);
            store.TryAdd(request.Key, new NewsModel
            {
                Who = request.Who,
                When = request.When,
                Story = request.Story
            });

            return Task.FromResult(new NewsAddResponse { Key = request.Key });
        }
    }
}
