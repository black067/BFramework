
namespace BFramework.ExpandedMath
{
    /// <summary>
    /// 分段数组
    /// </summary>
    [System.Serializable]
    public class Segments
    {
        public Segments(params int[] args)
        {
            Count = args.Length;
            Weights = args;
            Sum = 0;
            Accumulations = new int[Count];
            Max = int.MinValue;
            Min = int.MaxValue;
            for(int i = 0; i < Count; i++)
            {
                Accumulations[i] = Sum;
                Sum += args[i];
                if (Max <= args[i]) { Max = args[i]; }
                if (Min >= args[i]) { Min = args[i]; }
            }
        }

        public int Count { get; private set; }
        public int Max { get; private set; }
        public int Min { get; private set; }

        public int[] Weights { get; private set; }

        public int[] Accumulations { get; private set; }

        public int Sum { get; private set; }

        public int this[int value]
        {
            get
            {
                int index = 0;
                for (; index < Count - 1; index++)
                {
                    if(value <= Accumulations[index + 1]) { break; }
                }
                return index;
            }
        }

        public int GetRandomIndex()
        {
            return this[Random.Range(0, Sum)];
        }

        public override string ToString()
        {
            string result = "Segments(";
            int length = Weights.Length;
            for (int i = 0; i < length - 1; i++)
            {
                result += Weights[i] + ", ";
            }
            result += Weights[length - 1] + ")";
            return result;
        }
    }
}
