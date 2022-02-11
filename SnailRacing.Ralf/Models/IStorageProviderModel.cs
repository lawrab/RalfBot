namespace SnailRacing.Ralf.Models
{
    public interface IStorageProviderModel
    {
        void SetSaveDataCallback(Action saveData);
    }
}
