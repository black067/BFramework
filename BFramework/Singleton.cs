using System;
using System.Reflection;

namespace BFramework
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        protected static T _instance = null;

        protected static bool _initialized;

        protected Singleton() { }

        public static T Instance
        {
            get
            {
                if (!_initialized && _instance == null)
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
