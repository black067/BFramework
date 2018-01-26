using System;

namespace BFramework
{
    public class BDelegate<T>
    {
        public BDelegate(Method method)
        {
            this.method = method;
        }

        public delegate T Method(params Object[] __args);

        public Method method;
    }
}
