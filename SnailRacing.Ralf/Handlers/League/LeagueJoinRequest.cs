using SnailRacing.Ralf.Infrastrtucture;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueJoinRequest : IRequest<LeagueJoinResponse>
    {
        public string LeagueName { get; set; }
        public string DiscordMemberId { get; set; }
    }
}
