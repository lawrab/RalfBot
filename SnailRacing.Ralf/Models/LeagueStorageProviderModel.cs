using System.Collections;
using System.Collections.Concurrent;

namespace SnailRacing.Ralf.Models
{
    public class LeagueStorageProviderModel : StorageProviderModelBase<ConcurrentDictionary<string, LeagueModel>>, 
        IEnumerable<KeyValuePair<string, LeagueModel>>
    {
        public LeagueStorageProviderModel()
        {
            _data = new ConcurrentDictionary<string, LeagueModel>();
        }

        public LeagueModel? this[string key]
        {
            get
            {
                return _data?[key];
            }
            set
            {
                if (_data is null) throw new ArgumentException("_data property not initialised");
                _data[key] = value!;
                _saveData();
            }
        }

        public IEnumerator<KeyValuePair<string, LeagueModel>> GetEnumerator()
        {
            if (_data is null) return Enumerable.Empty<KeyValuePair<string, LeagueModel>>().GetEnumerator();

            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_data is null) return Enumerable.Empty<KeyValuePair<string, string>>().GetEnumerator();

            return _data.GetEnumerator();
        }
    }
}
