using MediatR;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueJoinRequest : LeagueRequestBase, IRequest<LeagueJoinResponse>
    {
        public string DiscordMemberId { get; set; } = string.Empty;
    }
}
