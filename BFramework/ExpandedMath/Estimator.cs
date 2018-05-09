using System;
using System.Collections.Generic;
namespace BFramework.ExpandedMath
{
    [Serializable]
    public class Estimable
    {
        /// <summary>
        /// 用于保存键与数值的字典
        /// </summary>
        public Dictionary<string, int> Dictionary { get; set; }

        /// <summary>
        /// 使用一个已有的字典初始化 Estimable
        /// </summary>
        /// <param name="dictionary"></param>
        public Estimable(Dictionary<string, int> dictionary)
        {
            Dictionary = dictionary;
        }

        /// <summary>
        /// 初始化一个空 Estimable
        /// </summary>
        public Estimable() : this(new Dictionary<string, int>()) { }

        /// <summary>
        /// 使用键取得数值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int this[string key]
        {
            get
            {
                if (!Dictionary.ContainsKey(key))
                    Dictionary.Add(key, 0);
                return Dictionary[key];
            }
            set
            {
                if (!Dictionary.ContainsKey(key))
                    Dictionary.Add(key, value);
                Dictionary[key] = value;
            }
        }
        
        /// <summary>
        /// 获取字典的键值列表
        /// </summary>
        public Dictionary<string, int>.KeyCollection Keys { get { return Dictionary.Keys; } }

        public bool ContainsKey(string key)
        {
            return Dictionary.ContainsKey(key);
        }

        /// <summary>
        /// 获取字典的总数量
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return Dictionary.Count;
        }

        /// <summary>
        /// 将加数中每个键对应的值与自身相应的值相加
        /// </summary>
        /// <param name="addition"></param>
        public void Add(Estimable addition)
        {
            foreach (string key in addition.Keys)
            {
                this[key] += addition[key];
            }
        }

        /// <summary>
        /// 将自身每个键对应的值与加数相加
        /// </summary>
        /// <param name="addition"></param>
        public void Add(int addition)
        {
            foreach (string key in Keys)
            {
                this[key] += addition;
            }
        }

        /// <summary>
        /// 将乘数中每个键对应的值与自身相应的值相乘
        /// </summary>
        /// <param name="multiplier"></param>
        public void Multiply(Estimable multiplier)
        {
            foreach (string key in multiplier.Keys)
            {
                this[key] *= multiplier[key];
            }
        }

        /// <summary>
        /// 将自身每个键对应的值与乘数相乘
        /// </summary>
        /// <param name="multiplier"></param>
        public void Multiply(int multiplier)
        {
            foreach (string key in Keys)
            {
                this[key] *= multiplier;
            }
        }

        /// <summary>
        /// 获取自身所有值的总和
        /// </summary>
        /// <returns></returns>
        public int Sum()
        {
            int result = 0;
            foreach (string key in Keys)
            {
                result += this[key];
            }
            return result;
        }

        /// <summary>
        /// 取得一个自身的克隆
        /// </summary>
        /// <returns></returns>
        public Estimable Clone()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            foreach (string key in Keys)
            {
                dictionary.Add(key, this[key]);
            }
            return new Estimable(dictionary);
        }
    }

    /// <summary>
    /// 估值器, 根据一组数值的权重计算总价值
    /// </summary>
    public class Estimator<T> where T : Estimable
    {
        public T WeightItem { get; set; }

        public Estimator(T weightItem)
        {
            WeightItem = weightItem;
        }

        public int this[T item]
        {
            get
            {
                return Calculate(item);
            }
        }

        public int Calculate(T item)
        {
            Estimable itemNew = WeightItem.Clone();
            itemNew.Multiply(item);
            return itemNew.Sum();
        }
    }
}
