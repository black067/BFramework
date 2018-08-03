using System;
using System.Collections.Generic;
using System.Linq;

namespace BFramework
{
    public interface IPool<T>
    {
        T Allocate();
        bool Recycle(T item);
    }

    public interface IPoolable
    {
        bool IsRecycled { get; set; }
        void OnRecycled();
        void OnAllocate();
        void OnCreated();
    }

    public interface IPoolType
    {
        void RecycleToCache();
    }
    
    /// <summary>
    /// 池的基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Pool<T> : IPool<T> where T : IPoolable, new()
    {
        protected int _maxCount { get; private set; }

        public int Threshold { get; set; }

        public int Count { get { return _cache.Count; } }

        public int Capcity
        {
            get { return _maxCount; }
            protected set
            {
                _maxCount = value;
                if (_cache != null && _maxCount > 0 && _maxCount < _cache.Count)
                {
                    for (int i = _cache.Count - _maxCount; i > 0; i--)
                    {
                        _cache.Pop();
                    }
                }
            }
        }

        public virtual T Allocate()
        {
            T item = _cache.Count == 0 ? _factory.Create() : _cache.Pop();
            item.OnAllocate();
            return item;
        }

        public abstract bool Recycle(T item);

        protected IFactory<T> _factory;

        protected Stack<T> _cache = new Stack<T>();
    }

    /// <summary>
    /// 安全对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SafePool<T> : Pool<T>, ISingleton where T : IPoolable, new()
    {
        #region 实现接口 ISingleton
        public virtual void OnInitialized()
        {
            _factory = new Factory<T>();
            Init(64, 64);
        }

        protected SafePool() { }

        public static SafePool<T> Instance
        {
            get
            {
                return SingletonProperty<SafePool<T>>.Instance;
            }
        }

        public void Dispose()
        {
            SingletonProperty<SafePool<T>>.Dispose();
        }
        #endregion

        #region 对基类 Pool<T> 的拓展
        public override T Allocate()
        {
            T item = base.Allocate();
            item.IsRecycled = false;
            return item;
        }

        public override bool Recycle(T item)
        {
            if(item == null || item.IsRecycled) { return false; }
            if(Capcity > 0 &&_cache.Count >= Capcity)
            {
                item.OnRecycled();
                return false;
            }
            item.IsRecycled = true;
            item.OnRecycled();
            _cache.Push(item);
            return true;
        }
        #endregion

        public void Init(int capacity = 64, int count = 64)
        {
            if (capacity > 0)
            {
                count = capacity <= count ? count : capacity;
                Capcity = capacity;
            }

            for (int i = Count; i < count; i++)
            {
                Recycle(_factory.Create());
            }
        }
    }
    
}
