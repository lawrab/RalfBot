﻿namespace SnailRacing.Ralf.Models
{
    public abstract class StorageProviderModelBase<T> : IStorageProviderModel
    {
        protected Action _saveData = () => { };
        protected T? _data;

        public T? InternalStore
        {
            get => _data;
        }

        public virtual void SetSaveDataCallback(Action saveData)
        {
            _saveData = saveData;
        }

        public virtual Type GetStoreType()
        {
            return typeof(T);
        }

        public void SetStore(object store)
        {
            if (store is not T) throw new ArgumentException($"Invalid argument type {nameof(store)} excpected type is {typeof(T)}");

            _data = (T)store;
        }

        public object GetStore()
        {
            return _data!;
        }
    }
}
