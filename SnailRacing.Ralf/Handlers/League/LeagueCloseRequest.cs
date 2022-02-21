using MediatR;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueCloseRequest : LeagueRequestBase, IRequest<LeagueCloseResponse>
    {
    }
}
