using MediatR;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueJoinRequest : IRequest<LeagueJoinResponse>
    {
        public string LeagueName { get; set; } = string.Empty;
        public string DiscordMemberId { get; set; } = string.Empty;
    }
}
