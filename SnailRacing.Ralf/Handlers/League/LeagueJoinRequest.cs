using MediatR;

namespace SnailRacing.Ralf.Handlers.League
{
    public class LeagueJoinRequest : IRequest<LeagueJoinResponse>
    {
        public string LeagueName { get; set; } = string.Empty;
        public string DiscordMemberId { get; set; } = string.Empty;
        public string GuildId { get; set; } = string.Empty;

        public string LeagueKey { get => $"{GuildId}-{LeagueName}"; }
    }
}
