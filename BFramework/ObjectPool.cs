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

        public ObjectPool(int capacity = 10)
        {
            _pool = new Dictionary<object, Queue<object>>(capacity);
            _types = new Dictionary<object, Type>(capacity);
            _caches = new Dictionary<object, int>(capacity);
            _tags = new Dictionary<object, object>(capacity);
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

        /// <summary>
        /// 池的类型表
        /// </summary>
        public List<Type> Types
        {
            get => new List<Type>(_types.Values);
        }

        /// <summary>
        /// 获取键值对应队列的元素数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// 清空对象池
        /// </summary>
        public void Clear()
        {
            _pool.Clear();
            _tags.Clear();
        }

        /// <summary>
        /// 根据键值，类型创建新池
        /// </summary>
        /// <param name="key"></param>
        /// <param name="itemType"></param>
        /// <param name="cache"></param>
        public void CreateNewQueue(Object key, Type itemType, int cache = 1)
        {
            if (_pool.ContainsKey(key))
            {
                return;
            }
            _pool.Add(key, new Queue<object>());
            _caches.Add(key, cache > 1 ? cache : 1);
            _types.Add(key, itemType);
        }

        /// <summary>
        /// 根据键值，已有的数组创建新池
        /// </summary>
        /// <param name="key"></param>
        /// <param name="array"></param>
        /// <param name="cache"></param>
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

        /// <summary>
        /// 从键值对应的队列中获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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
                Fill(ref key);
                return _pointer;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 返还对象到池中
        /// </summary>
        /// <param name="item"></param>
        public void Restore(Object item)
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

        /// <summary>
        /// 将对象标记为出借
        /// </summary>
        /// <param name="item"></param>
        /// <param name="key"></param>
        private void MarkAsOut(ref Object item, Object key)
        {
            _tags.Add(item, key);
        }

        /// <summary>
        /// 补充对象池
        /// </summary>
        /// <param name="key"></param>
        private void Fill(ref Object key)
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
