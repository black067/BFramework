using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework
{
    /// <summary>
    /// 对象工厂接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFactory<T>
    {
        T Create();
    }
    
    public class Factory<T> : IFactory<T> where T : IPoolable, new()
    {
        public T Create()
        {
            T item = new T();
            item.OnCreated();
            return item;
        }
    }
}
