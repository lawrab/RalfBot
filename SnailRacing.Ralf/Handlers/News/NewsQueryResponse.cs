using SnailRacing.Ralf.Infrastrtucture;

namespace SnailRacing.Ralf.Handlers.News
{
    public class NewsQueryResponse : ResponseBase
    {
        public IEnumerable<NewsModel> News { get; set; } = Enumerable.Empty<NewsModel>();
    }
}
