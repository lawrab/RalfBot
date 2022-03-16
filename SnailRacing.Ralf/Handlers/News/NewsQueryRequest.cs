using MediatR;

namespace SnailRacing.Ralf.Handlers.News
{
    public class NewsQueryRequest : IRequest<NewsQueryResponse>
    {
        public string GuildId { get; set; } = string.Empty;
        public Predicate<NewsModel> Filter { get; set; } = (m) => true;
    }
}
