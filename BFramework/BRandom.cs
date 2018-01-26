using System;

namespace BFramework
{
    public static class BRandom
    {
        static Random _randomCast = new Random();
        static int _seed = 0;

        public static int Seed { get => _seed; private set => _seed = value; }

        /// <summary>
        /// 返回一个范围内的随机整数。
        /// 注意：返回值 ∈ [min, max)
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
            int range = max - min;
            int next = _randomCast.Next(range);
            next += min;
            return next;
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
            int sum = 0;
            int sum2 = 0;
            for (int i = number - 1; i > -1; i--)
            {
                array[i] = _randomCast.Next(1, 100000);
                sum += array[i];
            }
            for (int i = number - 1; i > -1; i--)
            {
                array[i] = range * array[i] / sum;
                sum2 += array[i];
            }
            if (sum2 < range)
            {
                for (int i = range - sum2 - 1; i > -1; i--)
                {
                    array[Range(0, number)] ++;
                }
            }

            return array;
        }

        public static void Init(int seed)
        {
            Seed = seed;
            _randomCast = new Random(Seed);
        }

        public static void Init()
        {
            _randomCast = new Random();
        }
    }
}
