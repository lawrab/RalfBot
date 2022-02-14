using System.Collections.Concurrent;

namespace SnailRacing.Ralf.Models
{
    public class NewsStorageProviderModel : StorageProviderModelBase<ConcurrentBag<NewsModel>>
    {
        public NewsStorageProviderModel()
        {
            _data = new ConcurrentBag<NewsModel>();
        }

        public void SetSaveDataCallback(Action saveData)
        {
            _saveData = saveData;
        }

        public void Add(NewsModel news)
        {
            _data?.Add(news);
            _saveData();
        }

        public IEnumerable<NewsModel>? Query(int count)
        {
            return _data?.OrderByDescending(n => n.When)
                .Take(count);
        }

        public override void SetStore(object store)
        {
            var storeArray = store as NewsModel[] ?? Array.Empty<NewsModel>();
            var data = new ConcurrentBag<NewsModel>(storeArray);
            base.SetStore(data);
        }

        public override Type GetStoreType()
        {
            return typeof(NewsModel[]);
        }
    }
}
