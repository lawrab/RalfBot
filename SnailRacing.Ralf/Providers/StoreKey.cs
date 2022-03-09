namespace SnailRacing.Ralf.Providers
{
    public class StoreKey
    {
        public StoreKey(string guildId, string key)
        {
            GuildId = guildId;
            Key = key;
        }

        public string GuildId { get; }
        public string Key { get; }
    }
}
