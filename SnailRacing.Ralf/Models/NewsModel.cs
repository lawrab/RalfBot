namespace SnailRacing.Ralf.Models
{
    public class NewsModel
    {
        public DateTime When { get; set; } = DateTime.UtcNow;
        public string Who { get; set; } = string.Empty;
        public string Story { get; set; } = string.Empty;
    }
}