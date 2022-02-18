using MediatR;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueNewRequest : IRequest<LeagueNewResponse>
    {
        public string GuildId { get; set; } = string.Empty;
        public string LeagueName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LeagueKey { get => $"{GuildId}-{LeagueName}"; }
    }
}
