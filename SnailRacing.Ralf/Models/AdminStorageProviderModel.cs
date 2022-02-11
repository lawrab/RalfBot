using DSharpPlus.Entities;

namespace SnailRacing.Ralf.Models
{
    internal class AdminStorageProviderModel : IStorageProviderModel
    {
        private Action _saveData = () => { };

        public DiscordChannel? LoggingChannel { get; set; }

        public void SetSaveDataCallback(Action saveData)
        {
            _saveData = saveData;           
        }
    }
}
