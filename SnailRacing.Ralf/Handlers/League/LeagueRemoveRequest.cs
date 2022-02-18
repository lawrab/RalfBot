using MediatR;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueRemoveRequest : IRequest<LeagueRemoveResponse>
    {
        public string LeagueName { get; set; } = string.Empty;
        public string GuildId { get; internal set; }
    }
}
