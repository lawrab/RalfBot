using System.Collections.Concurrent;

namespace SnailRacing.Ralf.Models
{
    public class LeagueModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public LeagueStatus Status { get; set; }
        public ConcurrentBag<LeagueParticipantModel> Participants { get; set; } = new ConcurrentBag<LeagueParticipantModel>();
        public DateTime CreatedDate { get; set; }
        public Uri? Standings { get; set; } = new Uri("https://annieandlarry.com");
    }
}
