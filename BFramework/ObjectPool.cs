using System;
using System.Collections.Generic;
using System.Linq;

namespace BFramework
{

    /// <summary>
    /// 对象池应具备的方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPool<T>
    {

        /// <summary>
        /// 取得物体
        /// </summary>
        /// <returns></returns>
        T Allocate();

        /// <summary>
        /// 回收物体
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Recycle(T item);
    }

    /// <summary>
    /// 可被对象池收容的类应具备的属性与方法
    /// </summary>
    public interface IPoolable
    {

        /// <summary>
        /// 是否被回收
        /// </summary>
        bool IsRecycled { get; set; }

        /// <summary>
        /// 被回收时的动作
        /// </summary>
        void OnRecycled();

        /// <summary>
        /// 被取出时的动作
        /// </summary>
        void OnAllocate();

        /// <summary>
        /// 被制造时的动作
        /// </summary>
        void OnCreated();
    }
    
    /// <summary>
    /// 池的基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Pool<T> : IPool<T> where T : IPoolable, new()
    {

        /// <summary>
        /// 最大值
        /// </summary>
        protected int _maxCount { get; private set; }

        /// <summary>
        /// 触发对象池填充的阈值
        /// </summary>
        public int Threshold { get; set; }

        /// <summary>
        /// 池内物体的数量
        /// </summary>
        public int Count { get { return _cache.Count; } }

        /// <summary>
        /// 对象池的容量, 改变其容量可能会触发对象池的释放动作
        /// </summary>
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

        /// <summary>
        /// 取得物体
        /// </summary>
        /// <returns></returns>
        public virtual T Allocate()
        {
            T item = _cache.Count == 0 ? _factory.Create() : _cache.Pop();
            item.OnAllocate();
            return item;
        }

        /// <summary>
        /// 回收物体
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public abstract bool Recycle(T item);

        /// <summary>
        /// 工厂对象, 用于创建物体
        /// </summary>
        protected IFactory<T> _factory;

        /// <summary>
        /// 对象池维护的堆栈
        /// </summary>
        protected Stack<T> _cache = new Stack<T>();
    }

    /// <summary>
    /// 安全对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SafePool<T> : Pool<T>, ISingleton where T : IPoolable, new()
    {
        #region 实现 ISingleton 接口

        /// <summary>
        /// 初始化时的动作
        /// </summary>
        public virtual void OnInitialized()
        {
            _factory = new Factory<T>();
            Init(64, 64);
        }

        /// <summary>
        /// 受保护的无参构建方法
        /// </summary>
        protected SafePool() { }

        /// <summary>
        /// 取得全局唯一实例
        /// </summary>
        public static SafePool<T> Instance
        {
            get
            {
                return SingletonProperty<SafePool<T>>.Instance;
            }
        }

        /// <summary>
        /// 释放全局唯一的实例
        /// </summary>
        public void Dispose()
        {
            SingletonProperty<SafePool<T>>.Dispose();
        }
        #endregion

        #region 对基类 Pool<T> 的拓展

        /// <summary>
        /// 取得物体
        /// </summary>
        /// <returns></returns>
        public override T Allocate()
        {
            T item = base.Allocate();
            item.IsRecycled = false;
            return item;
        }

        /// <summary>
        /// 回收物体
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 给定容量与初始物体数目初始化对象池
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="count"></param>
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
