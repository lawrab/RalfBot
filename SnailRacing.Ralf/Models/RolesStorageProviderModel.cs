using System.Collections;
using System.Collections.Concurrent;

namespace SnailRacing.Ralf.Models
{
    public class RolesStorageProviderModel : StorageProviderModelBase<ConcurrentDictionary<string, string>>, IEnumerable<KeyValuePair<string, string>>
    {
        public RolesStorageProviderModel()
        {
            _data = new ConcurrentDictionary<string, string>();
        }

        public string? this[string key]
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

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            if (_data is null) return Enumerable.Empty<KeyValuePair<string, string>>().GetEnumerator();

            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_data is null) return Enumerable.Empty<KeyValuePair<string, string>>().GetEnumerator();

            return _data.GetEnumerator();
        }
    }
}
