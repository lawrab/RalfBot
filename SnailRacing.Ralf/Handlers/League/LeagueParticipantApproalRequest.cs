using MediatR;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueParticipantApproalRequest : LeagueRequestBase, IRequest<LeagueParticipantApprovalResponse>
    {
        public string DiscordMemberId { get; set; } = string.Empty;
        public string ApprovedBy { get; set; } = string.Empty;
    }
}
