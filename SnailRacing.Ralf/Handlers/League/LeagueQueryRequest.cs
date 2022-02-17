using MediatR;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueQueryRequest : IRequest<LeagueQueryResponse>
    {
        public Predicate<LeagueModel> Query { get; set; } = (m) => true;
    }
}
