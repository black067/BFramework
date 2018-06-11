using System;
using System.Reflection;

namespace BFramework
{
    /// <summary>
    /// 单例接口
    /// </summary>
    public interface ISingleton
    {
        void OnInitialized();
    }

    /// <summary>
    /// 单例属性器, 无需继承单例基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonProperty<T> where T : class, ISingleton
    {
        protected static T _instance = null;
        private static object _lock = new object();

        /// <summary>
        /// 取得单例
        /// </summary>
        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = CreateSingleton();
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// 释放单例
        /// </summary>
        public static void Dispose()
        {
            _instance = null;
        }

        /// <summary>
        /// 用于创建单例
        /// </summary>
        /// <returns></returns>
        private static T CreateSingleton()
        {
            T item = default(T);

            ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

            if (ctor == null)
            {
                throw new Exception("Non-public ctor() not found! in " + typeof(T));
            }

            item = ctor.Invoke(null) as T;

            item.OnInitialized();

            return item;
        }
    }

    public abstract class Singleton<T> where T : Singleton<T>
    {
        protected static T _instance = null;

        protected static bool _initialized = false;

        protected Singleton() { }
        
        public static T Instance
        {
            get
            {
                if (!_initialized || _instance == null)
                {
                    ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                    _instance = ctor.Invoke(null) as T;
                    _initialized = true;
                }
                return _instance;
            }
        }
    }
}
