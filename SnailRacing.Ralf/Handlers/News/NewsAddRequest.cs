using MediatR;

namespace SnailRacing.Ralf.Handlers.News
{
    public class NewsAddRequest : IRequest<NewsAddResponse>
    {
        public string GuildId { get; set; } = string.Empty;
        public DateTime When { get; set; } = DateTime.UtcNow;
        public string Who { get; set; } = string.Empty;
        public string Story { get; set; } = string.Empty;
        public string Key { get; } = Guid.NewGuid().ToString();
    }
}
