using MediatR;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueRemoveRequest : LeagueRequestBase, IRequest<LeagueRemoveResponse>
    {
    }
}
