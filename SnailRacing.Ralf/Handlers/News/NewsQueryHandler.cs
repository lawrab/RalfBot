using MediatR;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers.News
{
    public class NewsQueryHandler : IRequestHandler<NewsQueryRequest, NewsQueryResponse>
    {
        private readonly IStorageProvider _storageProvider;

        public NewsQueryHandler(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }
        public Task<NewsQueryResponse> Handle(NewsQueryRequest request, CancellationToken cancellationToken)
        {
            var newsStore = StoreHelper.GetNewsStore(request.GuildId, _storageProvider);

            var news = newsStore.OrderByDescending(n => n.Value.When)
                .Where(m => request.Filter(m.Value))
                .Take(5) // only ever return the top 10 news items
                .Select(m => m.Value)
                .ToList();
            return Task.FromResult(new NewsQueryResponse
            {
                News = news
            });
        }
    }
}
