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
        /// 取得实例
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
    
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class NeedResetAttribute : Attribute
    {
        public bool ResetValueToNull;
        public NeedResetAttribute(bool ResetValueToNull)
        {
            this.ResetValueToNull = ResetValueToNull;
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

        /// <summary>
        /// 重置单例中所有标记了 NeedReset 属性的 Field
        /// </summary>
        public static void Reset()
        {
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (FieldInfo f in fields)
            {
                object[] objs = f.GetCustomAttributes(typeof(NeedResetAttribute), false);
                if (objs.Length > 0)
                {
                    var o = objs[0] as NeedResetAttribute;
                    if (o.ResetValueToNull)
                    {
                        f.SetValue(Instance, null);
                    }
                    else
                    {
                        ConstructorInfo[] ctrs = f.FieldType.GetConstructors();
                        ConstructorInfo ctr = Array.Find(f.FieldType.GetConstructors(), c => c.GetParameters().Length == 0);
                        f.SetValue(Instance, ctr.Invoke(null));
                    }
                }
            }
        }
    }
}
