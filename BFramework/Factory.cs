using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework
{
    /// <summary>
    /// 对象工厂需要具备的方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFactory<T>
    {

        /// <summary>
        /// 制造实例
        /// </summary>
        /// <returns></returns>
        T Create();
    }
    
    /// <summary>
    /// 对象工厂类, 泛型参数是可收入对象池且可以无参构建的类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Factory<T> : IFactory<T> where T : IPoolable, new()
    {

        /// <summary>
        /// 制造实例
        /// </summary>
        /// <returns></returns>
        public T Create()
        {
            T item = new T();
            item.OnCreated();
            return item;
        }
    }
}
