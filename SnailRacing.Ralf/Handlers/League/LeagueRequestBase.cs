using SnailRacing.Ralf.Infrastrtucture;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueRequestBase : RequestBase
    {
        public string LeagueName { get; set; } = string.Empty;

        public string LeagueKey { get => $"{LeagueName.ToUpper()}"; }
    }
}
