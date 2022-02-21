using MediatR;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueOpenRequest : LeagueRequestBase, IRequest<LeagueOpenResponse>
    {
        public int? MaxGrid { get; set; }
    }
}
