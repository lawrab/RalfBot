using MediatR;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueLeaveRequest : LeagueRequestBase, IRequest<LeagueLeaveResponse>
    {
        public string DiscordMemberId { get; set; } = string.Empty;
    }
}
