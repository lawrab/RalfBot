namespace SnailRacing.Ralf.Models
{
    public class LeagueParticipantModel
    {
        public string DiscordMemberId { get; set; } = string.Empty;
        public string DicordMemberUser { get; set; } = string.Empty;
        public string IRacingName { get; set; } = string.Empty;
        public int IRacingCustomerId { get; set; }
        public bool AgreeTermsAndConditions { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public LeagueParticipantStatus Status { get; set; }
        public string? ApprovedBy { get; internal set; }
    }
}
