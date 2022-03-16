using MediatR;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueJoinRequest : LeagueRequestBase, IRequest<LeagueJoinResponse>
    {
        public string DiscordMemberId { get; set; } = string.Empty;
        public string DiscordMemberUser { get; set; } = string.Empty;
        public string IRacingName { get; set; } = string.Empty;
        public int IRacingCustomerId { get; set; }
        public bool AgreeTermsAndConditions { get; set; }
    }
}
