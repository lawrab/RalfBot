namespace SnailRacing.Ralf.Models
{
    public class GuildConfigModel
    {
        public string GuildId { get; set; } = string.Empty;
        public bool IsTailOn { get; set; } = false;

        public bool IsFactsOn { get; set; } = false;
        public ulong? LoggingChannelId { get; set; }
    }
}
