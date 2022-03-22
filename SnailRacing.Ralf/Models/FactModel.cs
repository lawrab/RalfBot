namespace SnailRacing.Ralf.Models
{
    public class FactModel
    {
        public Success? Success { get; set; }
        public Contents? Contents { get; set; }
    }
    public class Success
    {
        public int Total { get; set; }
    }

    public class Contents
    {
        public string? Fact { get; set; }
        public string? Id { get; set; }
        public string? Category { get; set; }
        public string? Subcategory { get; set; }
    }
}

