using System;

namespace BFramework.ExpandedMath
{
    public interface IEstimable<T>
    {
        int GetCount();
        void Add(IEstimable<T> addition);
        void Add(T addition);
        void Multiply(IEstimable<T> multiplier);
        void Multiply(T multiplier);
        IEstimable<T> Clone();
        T Sum();
    }
    /// <summary>
    /// 估值器, 根据一组数值的权重计算总价值
    /// </summary>
    public class Estimator<T> where T : IEstimable<int>
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
            T itemNew = (T)WeightItem.Clone();
            itemNew.Multiply(item);
            return itemNew.Sum();
        }
    }
}
