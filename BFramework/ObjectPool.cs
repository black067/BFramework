using System;
using System.Collections.Generic;

namespace BFramework
{
    public class ObjectPool
    {
        private interface IShellObject
        {
            void Create(Object param);
            Object GetInnerObject();
            bool IsValidate();
            void Release();
        }

        private class ShellObject
        {
            private IShellObject _shell;
            private bool _beUsing;
            private Type _type;
            private Object _createParam;

            public ShellObject(Type type, Object param)
            {
                _type = type;
                _createParam = param;
                Create();
            }

            private void Create()
            {
                _beUsing = false;
                _shell = (IShellObject)Activator.CreateInstance(_type);
                _shell.Create(_createParam);
            }
        }

        private Dictionary<Object, Queue<Object>> _pool = new Dictionary<object, Queue<object>>();
        private Dictionary<Object, Type> _types = new Dictionary<object, Type>();
        private Dictionary<Object, int> _caches = new Dictionary<object, int>();
        private Dictionary<Object, Object> _tags = new Dictionary<object, object>();
        private Object _pointer;
        
        public List<Object> GetKeys()
        {
            return new List<Object>(_pool.Keys);
        }

        public int GetCount(Object key)
        {
            if (_pool.ContainsKey(key))
            {
                return _pool[key].Count;
            }
            else
            {
                return 0;
            }
        }
        
        public void Clear()
        {
            _pool.Clear();
            _tags.Clear();
        }

        public void CreateNewQueue(Object key, Type itemType, int cache)
        {
            _pool.Add(key, new Queue<object>());
            _caches.Add(key, cache > 1 ? cache : 1);
            _types.Add(key, itemType);
        }

        public void Add(Object key, Object item)
        {
            if (!_pool.ContainsKey(key))
            {
                CreateNewQueue(key, item.GetType(), 2);
            }
            _pool[key].Enqueue(item);
        }

        public Object GetItem(Object key)
        {
            if (_pool.ContainsKey(key))
            {
                _pointer = _pool[key].Dequeue();
                if (_pointer == null)
                {
                    return null;
                }
                MarkAsOut(_pointer, key);
                return _pointer;
            }
            else
            {
                return null;
            }
        }

        public void RestoreItem(Object item)
        {
            if (item == null)
            {
                return;
            }
            if (_tags.ContainsKey(item))
            {
                _pool[_tags[item]].Enqueue(item);
                _tags.Remove(item);
            }
        }

        private void MarkAsOut(Object item, Object key)
        {
            _tags.Add(item, key);
        }

        private void FillPool(Object key)
        {
            if(_pool[key].Count < _caches[key])
            {
                for (int i = _caches[key] - _pool[key].Count; i >= 0; i--)
                {
                    _pool[key].Enqueue(Activator.CreateInstance(_types[key]));
                }
            }
        }
    }
}
