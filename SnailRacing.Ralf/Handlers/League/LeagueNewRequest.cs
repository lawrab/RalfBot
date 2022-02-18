using MediatR;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueNewRequest : LeagueRequestBase, IRequest<LeagueNewResponse>
    {
        public string Description { get; set; } = string.Empty;
    }
}
