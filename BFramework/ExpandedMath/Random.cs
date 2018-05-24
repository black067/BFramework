
namespace BFramework.ExpandedMath
{
    public static class Random
    {
        private static System.Random _randomCast = new System.Random();

        /// <summary>
        /// 随机数生成器种子（只读）
        /// </summary>
        public static int Seed { get; private set; }

        /// <summary>
        /// 取得一个随机浮点数
        /// 返回值 ∈ [min, max)
        /// </summary>
        public static float  Value
        {
            get
            {
                return (float)_randomCast.NextDouble();
            }
        }

        /// <summary>
        /// 返回一个范围内的随机双精度浮点数. 
        /// 返回值 ∈ [min, max)
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double Range(double min, double max)
        {
            if (min > max)
            {
                double m = max;
                max = min;
                min = m;
            }
            return _randomCast.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// 返回一个范围内的随机浮点数. 
        /// 返回值 ∈ [min, max)
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Range(float min, float max)
        {
            if (min > max)
            {
                float m = max;
                max = min;
                min = m;
            }
            return (float)_randomCast.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// 返回一个范围内的随机整数. 
        /// 返回值 ∈ [min, max)
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Range(int min, int max)
        {
            if (min > max)
            {
                int m = max;
                max = min;
                min = m;
            }
            return _randomCast.Next(max - min) + min;
        }
        
        /// <summary>
        /// 根据给定的权重数组, 随机返回一个索引
        /// </summary>
        /// <param name="weights"></param>
        /// <returns></returns>
        public static int GetIndex(params double[] weights)
        {
            double sum = 0;
            int length = weights.Length;
            double[] temp = new double[length];
            for(int i = 0; i < length; i++)
            {
                temp[i] = sum;
                sum += weights[i];
            }
            double num = Range(0, sum);
            int index = 0;
            for(; index < length - 1; index++)
            {
                if(num <= temp[index + 1]) { break; }
            }
            return index;
        }
        
        /// <summary>
        /// 使用种子初始化随机数生成器
        /// </summary>
        /// <param name="seed"></param>
        public static void Init(int seed)
        {
            Seed = seed;
            _randomCast = new System.Random(Seed);
        }

        /// <summary>
        /// 使用当前时间作为种子初始化随机数生成器
        /// </summary>
        public static void Init()
        {
            _randomCast = new System.Random();
        }
    }
}
