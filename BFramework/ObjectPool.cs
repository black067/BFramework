using System;
using System.Collections.Generic;
using System.Collections;

namespace BFramework
{
    public class ObjectPool
    {
        private Dictionary<Object, Queue<Object>> _pool = new Dictionary<object, Queue<object>>();
        private Dictionary<Object, Object> _tag = new Dictionary<object, object>();
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
            _tag.Clear();
        }

        public void Add(Object key, Object item)
        {
            if (!_pool.ContainsKey(key))
            {
                _pool.Add(key, new Queue<object>());
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
            RemoveMark(item);
        }

        private void MarkAsOut(Object item, Object key)
        {
            _tag.Add(item, key);
        }

        private void RemoveMark(Object item)
        {
            if (_tag.ContainsKey(item))
            {
                _pool[_tag[item]].Enqueue(item);
                _tag.Remove(item);
            }
        }
    }
}
