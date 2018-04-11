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
        private int _count;
        private T _weightItem;
        private int _normalizedValue;
        
        public int Count { get { return _count; } private set { _count = value; } }
        public T WeightItem { get { return _weightItem; } set { _weightItem = value; } }
        public int NormalizedValue { get { return _normalizedValue; } set { _normalizedValue = value; } }

        public Estimator(T weightItem, int normalizedValue = 10000)
        {
            WeightItem = weightItem;
            NormalizedValue = normalizedValue;
            Count = WeightItem.GetCount();
            Normalized();
        }

        private void Normalized()
        {
            int sum = _weightItem.Sum();
            int multiplier = NormalizedValue / sum;
            int remainder = NormalizedValue % sum / Count;
            _weightItem.Multiply(multiplier);
            _weightItem.Add(remainder);
            return;
        }

        public int Calculate(T item)
        {
            T itemNew = (T)WeightItem.Clone();
            Console.WriteLine(itemNew);
            itemNew.Multiply(item);
            return itemNew.Sum();
        }
    }
}
