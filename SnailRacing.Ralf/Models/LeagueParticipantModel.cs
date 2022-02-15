namespace SnailRacing.Ralf.Models
{
    public class LeagueParticipantModel
    {
        public string IRacingName { get; set; } = string.Empty;
        public int IRacingClientId { get; set; }
        public string DiscordNickname { get; set; } = string.Empty;
        public bool AgreeTermsAndConditions { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime? RegistrationDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public LeagueParticipantStatus Status { get; set; }
    }
}
