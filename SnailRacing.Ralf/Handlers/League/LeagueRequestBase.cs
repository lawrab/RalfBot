namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueRequestBase
    {
        public string LeagueName { get; set; } = string.Empty;
        public string GuildId { get; set; } = string.Empty;

        public string LeagueKey { get => $"{GuildId}-{LeagueName}"; }
    }
}
