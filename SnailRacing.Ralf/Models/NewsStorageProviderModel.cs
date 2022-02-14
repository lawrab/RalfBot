using System.Collections.Concurrent;

namespace SnailRacing.Ralf.Models
{
    public class NewsStorageProviderModel : StorageProviderModelBase<ConcurrentBag<NewsModel>>
    {
        private Action _saveData = () => { };
        private readonly ConcurrentBag<NewsModel> _data = new ConcurrentBag<NewsModel>();

        public void SetSaveDataCallback(Action saveData)
        {
            _saveData = saveData;
        }

        public void Add(NewsModel news)
        {
            _data.Add(news);
        }

        public IEnumerable<NewsModel> Query(int count)
        {
            return _data.OrderByDescending(n => n.When)
                .Take(count);
        }
    }
}
