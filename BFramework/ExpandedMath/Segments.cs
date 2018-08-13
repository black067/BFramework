using System.Runtime.Serialization;

namespace BFramework.ExpandedMath
{
    /// <summary>
    /// 分段数组
    /// </summary>
    [System.Serializable]
    public class Segments : ISerializable
    {

        /// <summary>
        /// 根据给定的 int 数组构造分段数组
        /// </summary>
        /// <param name="args"></param>
        public Segments(params int[] args)
        {
            Weights = args;
            Count = args.Length;
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

        /// <summary>
        /// 数组内元素的数量
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// 最大元素的值
        /// </summary>
        public int Max { get; private set; }

        /// <summary>
        /// 最小元素的值
        /// </summary>
        public int Min { get; private set; }

        /// <summary>
        /// 权重数组
        /// </summary>
        public int[] Weights { get; private set; }

        /// <summary>
        /// 逐步和数组
        /// </summary>
        public int[] Accumulations { get; private set; }

        /// <summary>
        /// 所有元素之和
        /// </summary>
        public int Sum { get; private set; }

        /// <summary>
        /// 根据给定的值, 返回比这个值小的最大的元素. 
        /// 例: 若分段数组的所有元素为 {0, 1, 2, 4}, 当所给的值为 3 时, 返回值是 2
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 在数组中随机取得一个元素
        /// </summary>
        /// <returns></returns>
        public int GetRandomIndex()
        {
            return this[BRandom.Range(0, Sum)];
        }

        /// <summary>
        /// 序列化为可读字符串
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Weights", Weights);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected Segments(SerializationInfo info, StreamingContext context)
        {
            int[] args = (int[])info.GetValue("Weights", typeof(int[]));
            Weights = args;
            Count = args.Length;
            Sum = 0;
            Accumulations = new int[Count];
            Max = int.MinValue;
            Min = int.MaxValue;
            for (int i = 0; i < Count; i++)
            {
                Accumulations[i] = Sum;
                Sum += args[i];
                if (Max <= args[i]) { Max = args[i]; }
                if (Min >= args[i]) { Min = args[i]; }
            }
        }
    }
}
