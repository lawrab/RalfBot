namespace SnailRacing.Ralf.Models
{
    public interface IStorageProviderModel
    {
        void SetSaveDataCallback(Action saveData);

        Type GetStoreType();

        void SetStore(object store);

        object GetStore();
    }
}
