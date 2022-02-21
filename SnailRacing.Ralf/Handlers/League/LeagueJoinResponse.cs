using SnailRacing.Ralf.Infrastrtucture;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueJoinResponse : ResponseBase
    {
        public bool MaxApprovedReached { get; set; }
    }
}
