using System;
using System.Reflection;

namespace BFramework
{
    /// <summary>
    /// 一个可以被单例属性器使用的类需要具备的方法
    /// </summary>
    public interface ISingleton
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void OnInitialized();
    }

    /// <summary>
    /// 单例属性器, 无需继承单例基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonProperty<T> where T : class, ISingleton
    {

        /// <summary>
        /// 全局唯一的实例
        /// </summary>
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
        protected static T CreateSingleton()
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
    
    /// <summary>
    /// 重置属性标记, 用于标记单例中初始化时需要重置的字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class NeedResetAttribute : Attribute
    {

        /// <summary>
        /// 是否需要将被标记的字段重置为 null
        /// </summary>
        public bool ResetValueToNull;

        /// <summary>
        /// 设置标记, 并指定该字段是否需要重置为 null
        /// </summary>
        /// <param name="ResetValueToNull"></param>
        public NeedResetAttribute(bool ResetValueToNull)
        {
            this.ResetValueToNull = ResetValueToNull;
        }
    }

    /// <summary>
    /// 单例基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> where T : Singleton<T>
    {

        /// <summary>
        /// 被维护的唯一对象实例
        /// </summary>
        protected static T _instance = null;

        /// <summary>
        /// 被维护的对象是否初始化
        /// </summary>
        protected static bool _initialized = false;

        /// <summary>
        /// 受保护的无参构建方法
        /// </summary>
        protected Singleton() { }
        
        /// <summary>
        /// 取得全局唯一实例
        /// </summary>
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
        /// 重置单例中所有被标记为 NeedReset 的字段
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
