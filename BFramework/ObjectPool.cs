using System;
using System.Collections;
using System.Collections.Generic;

namespace BFramework
{

    /// <summary>
    /// 对象池
    /// </summary>
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

            public void Recreate()
            {
                _shell.Release();
                Create();
            }

            public void Release()
            {
                _shell.Release();
            }
            
            public Object InnerObject
            {
                get => _shell.GetInnerObject();
            }

            public int InnerObjectHashcode
            {
                get => InnerObject.GetHashCode();
            }

            public bool IsValidate
            {
                get => _shell.IsValidate();
            }

            public bool Using
            {
                get => _beUsing;
                set => _beUsing = value;
            }
        }

        private Dictionary<Object, Queue<Object>> _pool = new Dictionary<object, Queue<object>>();
        private Dictionary<Object, Type> _types = new Dictionary<object, Type>();
        private Dictionary<Object, int> _caches = new Dictionary<object, int>();
        private Dictionary<Object, Object> _tags = new Dictionary<object, object>();
        private Object _pointer;
        
        
        /// <summary>
        /// 池的键值表
        /// </summary>
        public List<Object> Keys
        {
            get => new List<Object>(_pool.Keys);
        }

        public List<Type> Types
        {
            get => new List<Type>(_types.Values);
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
            if (_pool.ContainsKey(key))
            {
                return;
            }
            _pool.Add(key, new Queue<object>());
            _caches.Add(key, cache > 1 ? cache : 1);
            _types.Add(key, itemType);
        }
        public void CreateNewQueue(Object key, Queue<Object> queue, int cache = 1)
        {
            if (queue.Count < 1)
            {
                return;
            }
            CreateNewQueue(key, queue.Peek().GetType(), cache);
            for (int i = queue.Count - 1; i >= 0; i--)
            {
                _pool[key].Enqueue(queue.Dequeue());
            }
        }
        public void CreateNewQueue(Object key, List<Object> list, int cache = 1)
        {
            if(list.Count < 1)
            {
                return;
            }
            CreateNewQueue(key, list[0].GetType(), cache);
            for (int i = 0, length = list.Count; i < length; i++)
            {
                _pool[key].Enqueue(list[i]);
            }
        }
        public void CreateNewQueue(Object key, Array array, int cache = 1)
        {
            if (array.Length < 1)
            {
                return;
            }
            CreateNewQueue(key, array.GetValue(0).GetType(), cache);
            for (int i = 0, length = array.Length; i < length; i++)
            {
                _pool[key].Enqueue(array.GetValue(i));
            }
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
                MarkAsOut(ref _pointer, key);
                FillPool(ref key);
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

        private void MarkAsOut(ref Object item, Object key)
        {
            _tags.Add(item, key);
        }

        private void FillPool(ref Object key)
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
