using SnailRacing.Ralf.Infrastrtucture;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueQueryResponse : ResponseBase
    {
        public IEnumerable<LeagueModel> Leagues { get; set; } = Enumerable.Empty<LeagueModel>();
    }
}
