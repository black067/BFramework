using System;
using System.Collections.Generic;
using System.Linq;

namespace BFramework.Tools
{
    /// <summary>
    /// 对象池
    /// </summary>
    public class ObjectPool
    {
        /// <summary>
        /// 创建给定容量的对象池
        /// </summary>
        /// <param name="capacity"></param>
        public ObjectPool(int capacity = 10)
        {
            _pool = new Dictionary<object, Queue<object>>(capacity);
            _types = new Dictionary<object, Type>(capacity);
            _thresholds = new Dictionary<object, int>(capacity);
            _tags = new Dictionary<object, object>(capacity);
            Capacity = capacity;
        }

        /// <summary>
        /// 池的实例
        /// </summary>
        private Dictionary<Object, Queue<Object>> _pool { get; set; }

        /// <summary>
        /// 每个队列的类型字典
        /// </summary>
        private Dictionary<Object, Type> _types { get; set; }

        /// <summary>
        /// 记录池中每个队列的缓存阈值字典
        /// </summary>
        private Dictionary<Object, int> _thresholds { get; set; }

        /// <summary>
        /// 记录每个出借对象相应键值的字典
        /// </summary>
        private Dictionary<Object, Object> _tags { get; set; }
        private Object _pointer { get; set; }
        public int Capacity { get; private set; }
        
        /// <summary>
        /// 池的键值表
        /// </summary>
        public Dictionary<object, Queue<object>>.KeyCollection Keys
        {
            get
            {
                return _pool.Keys;
            }
        }

        /// <summary>
        /// 池的类型表
        /// </summary>
        public Dictionary<object,Type>.ValueCollection Types
        {
            get
            {
                return _types.Values;
            }
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
        /// <param name="threshold"></param>
        public void CreateNewQueue(Object key, Type itemType, int threshold = 2, int capacity = 20)
        {
            if (_pool.ContainsKey(key))
            {
                return;
            }
            _pool.Add(key, new Queue<object>(capacity));
            _thresholds.Add(key, threshold > 1 ? threshold : 1);
            _types.Add(key, itemType);
        }

        /// <summary>
        /// 根据键值，已有的数组创建新池
        /// </summary>
        /// <param name="key"></param>
        /// <param name="array"></param>
        /// <param name="threshold"></param>
        public void CreateNewQueue(Object key, Array array, int threshold = 2)
        {
            if (array.Length < 1)
            {
                return;
            }
            CreateNewQueue(key, array.GetValue(0).GetType(), threshold, array.Length);
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
                MarkAsOut(_pointer, key);
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
        private void MarkAsOut(Object item, Object key)
        {
            _tags.Add(item, key);
        }

        /// <summary>
        /// 补充对象池
        /// </summary>
        /// <param name="key"></param>
        private void Fill(ref Object key)
        {
            if(_pool[key].Count < _thresholds[key])
            {
                for (int i = _thresholds[key] - _pool[key].Count; i >= 0; i--)
                {
                    _pool[key].Enqueue(Activator.CreateInstance(_types[key]));
                }
            }
        }
    }
}
