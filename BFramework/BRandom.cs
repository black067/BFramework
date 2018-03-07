using System;

namespace BFramework
{
    public static class BRandom
    {
        static Random _randomCast = new Random();
        static int _seed = 0;

        /// <summary>
        /// 随机数生成器种子（只读）
        /// </summary>
        public static int Seed
        {
            get
            {
                return _seed;
            }
            private set
            {
                _seed = value;
            }
        }

        /// <summary>
        /// 返回一个范围内的随机整数。
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
        /// 根据给定的数量与给定数值，返回一个随机分布的正整数数组，数组所有元素之和等于给定数值
        /// </summary>
        /// <param name="number"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static int[] Distribution(int number, int range = 100)
        {
            number = number > range ? range : number;
            int[] array = new int[number];
            int powerSum = 0;
            int sum = 0;
            //随机产生权重并计算权重之和
            for (int i = number - 1; i > -1; i--)
            {
                array[i] = _randomCast.Next(1, int.MaxValue);
                powerSum += array[i];
            }
            //根据随机产生的权重与权重和，重新计算数组元素的数值，并计算新的数组元素和
            for (int i = number - 1; i > -1; i--)
            {
                array[i] = range * array[i] / powerSum;
                sum += array[i];
            }
            //补偿误差
            if (sum < range)
            {
                for (int i = range - sum - 1; i > -1; i--)
                {
                    array[Range(0, number)] ++;
                }
            }

            return array;
        }

        /// <summary>
        /// 使用种子初始化随机数生成器
        /// </summary>
        /// <param name="seed"></param>
        public static void Init(int seed)
        {
            Seed = seed;
            _randomCast = new Random(Seed);
        }

        /// <summary>
        /// 使用当前时间作为种子初始化随机数生成器
        /// </summary>
        public static void Init()
        {
            _randomCast = new Random();
        }
    }
}
