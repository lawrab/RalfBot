using SnailRacing.Ralf.Infrastrtucture;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueNewRequest : IRequest<LeagueNewResponse>
    {
        public string LeagueName { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
    }
}
