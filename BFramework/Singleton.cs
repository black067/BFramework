using System;
using System.Reflection;

namespace BFramework
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        protected static T _instance = null;

        protected Singleton() { }

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                    if (ctor == null)
                        throw new Exception("Non-public ctor() not found!");
                    _instance = ctor.Invoke(null) as T;
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
    }
}
