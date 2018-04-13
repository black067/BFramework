using System;
using System.Linq;
using System.Collections.Generic;
namespace BFramework.ExpandedMath
{
    [Serializable]
    public class Estimable
    {
        public Dictionary<string, int> Dictionary { get; set; }

        public string[] KeysArray { get; set; }

        public Estimable(Dictionary<string, int> dictionary)
        {
            Dictionary = dictionary;
            KeysArray = Dictionary.Keys.ToArray();
        }

        public Estimable() : this(new Dictionary<string, int>()) { }

        public int this[string key]
        {
            get { return Dictionary[key]; }
            set { Dictionary[key] = value; }
        }

        public int this[int index]
        {
            get { return Dictionary[KeysArray[index]]; }
            set { Dictionary[KeysArray[index]] = value; }
        }

        public Dictionary<string, int>.KeyCollection Keys { get { return Dictionary.Keys; } }

        public int GetCount()
        {
            return Dictionary.Count;
        }

        public void Add(Estimable addition)
        {
            foreach (string key in addition.Keys)
            {
                this[key] += addition[key];
            }
        }

        public void Add(int addition)
        {
            foreach (string key in Keys)
            {
                this[key] += addition;
            }
        }

        public void Multiply(Estimable multiplier)
        {
            foreach (string key in multiplier.Keys)
            {
                this[key] *= multiplier[key];
            }
        }

        public void Multiply(int multiplier)
        {
            foreach (string key in Keys)
            {
                this[key] *= multiplier;
            }
        }

        public int Sum()
        {
            int result = 0;
            foreach (string key in Keys)
            {
                result += this[key];
            }
            return result;
        }

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
        private T _weightItem;
        
        public T WeightItem { get { return _weightItem; } set { _weightItem = value; } }

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
