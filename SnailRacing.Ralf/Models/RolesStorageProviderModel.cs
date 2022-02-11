using System.Collections;
using System.Collections.Concurrent;

namespace SnailRacing.Ralf.Models
{
    public class RolesStorageProviderModel : IStorageProviderModel, IEnumerable<KeyValuePair<string, string>>
    {
        private Action _saveDataCallback = () => {};
        private readonly ConcurrentDictionary<string, string> _data = new ConcurrentDictionary<string, string>();

        public string this[string key]
        {
            get
            {
                return _data[key];
            }
            set
            {
                _data[key] = value;
                _saveDataCallback();
            }
        }

        public void SetSaveDataCallback(Action saveData)
        {
            _saveDataCallback = saveData;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}
