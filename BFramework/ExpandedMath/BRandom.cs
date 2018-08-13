using System;
using System.Collections.Generic;
using System.Linq;

namespace BFramework.ExpandedMath
{
    public static class BRandom
    {
        private static Random _randomCast = new Random();

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
            _randomCast = new Random(Seed);
        }

        /// <summary>
        /// 使用当前时间作为种子初始化随机数生成器
        /// </summary>
        public static void Init()
        {
            _randomCast = new Random();
        }

        /// <summary>
        /// 根据所给出现 true 的概率, 取得一个随机 Boolean 值
        /// </summary>
        /// <param name="trueProbability"></param>
        /// <returns></returns>
        public static bool GetBoolean(double trueProbability = 0.5)
        {
            if (Range(0f, 1f) <= trueProbability)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据给定的概率, 在给定的选择列表中选出一个结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="probabilities"></param>
        /// <returns></returns>
        public static T Choose<T>(IEnumerable<T> options, IEnumerable<int> probabilities)
        {
            int tempSum = 0;
            int length = options.Count();
            int[] p_SUM = new int[length];
            for (int i = 0; i < length; i++)
            {
                tempSum += probabilities.ElementAtOrDefault(i);
                p_SUM[i] = tempSum;
            }
            return Choose(options, tempSum, p_SUM);
        }

        /// <summary>
        /// 根据给定的概率之和以及概率的逐步和, 在给定的选项列表中选出一个结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="sum"></param>
        /// <param name="probabilities_sum"></param>
        /// <returns></returns>
        public static T Choose<T>(IEnumerable<T> options, int sum, IEnumerable<int> probabilities_sum)
        {
            int rnd = Range(0, sum), finalIndex = 0;
            for (int i = 0, count = options.Count(); i < count; i++)
            {
                if (rnd < probabilities_sum.ElementAtOrDefault(i))
                {
                    finalIndex = i;
                    break;
                }
            }
            return options.ElementAtOrDefault(finalIndex);
        }

        /// <summary>
        /// 在所给的值中随机选择一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg0"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T Choose<T>(T arg0, params T[] args)
        {
            int length = args.Length;
            int index = Range(0, length + 1);
            return (args.Length < 1 || index == length) ? arg0 : args[index];
        }

        /// <summary>
        /// 在所给的值中随机选择一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T Choose<T>(params T[] args)
        {
            if(args.Length < 1) { return default(T); }
            return args[Range(0, args.Length)];
        }


        /// <summary>
        /// 随机取得一个选项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        public static T GetElement<T>(IEnumerable<T> options)
        {
            return options.ElementAtOrDefault(Range(0, options.Count()));
        }

        /// <summary>
        /// 取得一个随机三维整数向量, 输入的值将依次作为 x 的最大/最小值, y 的最大/最小值, z 的最大/最小值, 输入的值最多 6 个, 超出部分无效. 
        /// 若输入三个值, 则随机的范围则是 x ∈ [0, args[0]), y ∈ [0, args[1]), z ∈ [0, args[2])
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static VectorInt GetVectorInt(int[] args)
        {
            VectorInt v = new VectorInt();
            switch (args.Length)
            {
                case 0:
                    return v;
                case 1:
                    v.x = Range(args[0], args[0] * 2);
                    return v;
                case 2:
                    v.x = Range(args[0], args[1]);
                    return v;
                case 3:
                    v.x = Range(0, args[0]);
                    v.y = Range(0, args[1]);
                    v.z = Range(0, args[2]);
                    return v;
                case 4:
                    v.x = Range(args[0], args[1]);
                    v.y = Range(args[2], args[3]);
                    return v;
                case 5:
                    v.x = Range(args[0], args[1]);
                    v.y = Range(args[2], args[3]);
                    v.z = Range(args[4], args[4] * 2);
                    return v;
                case 6:
                    v.x = Range(args[0], args[1]);
                    v.y = Range(args[2], args[3]);
                    v.z = Range(args[4], args[5]);
                    return v;
            }
            return v;
        }

        /// <summary>
        /// 根据给定的 x 的范围随机取得一个整数型向量
        /// </summary>
        /// <param name="xmin"></param>
        /// <param name="xmax"></param>
        /// <returns></returns>
        public static VectorInt GetVectorInt(int xmin, int xmax)
        {
            return new VectorInt()
            {
                x = Range(xmin, xmax),
            };
        }

        /// <summary>
        /// 根据给定的 x, y, z 的最大值随机取得一个整数型向量
        /// </summary>
        /// <param name="xmax"></param>
        /// <param name="ymax"></param>
        /// <param name="zmax"></param>
        /// <returns></returns>
        public static VectorInt GetVectorInt(int xmax, int ymax, int zmax)
        {
            return new VectorInt()
            {
                x = Range(0, xmax),
                y = Range(0, ymax),
                z = Range(0, zmax),
            };
        }

        /// <summary>
        /// 根据给定的 x, y 的范围随机取得一个整数型向量
        /// </summary>
        /// <param name="xmin"></param>
        /// <param name="xmax"></param>
        /// <param name="ymin"></param>
        /// <param name="ymax"></param>
        /// <returns></returns>
        public static VectorInt GetVectorInt(int xmin, int xmax, int ymin, int ymax)
        {
            return new VectorInt()
            {
                x = Range(xmin, xmax),
                y = Range(ymin, ymax),
            };
        }

        /// <summary>
        /// 根据给定的 x, y, z 的范围随机取得一个整数型向量
        /// </summary>
        /// <param name="xmin"></param>
        /// <param name="xmax"></param>
        /// <param name="ymin"></param>
        /// <param name="ymax"></param>
        /// <param name="zmin"></param>
        /// <param name="zmax"></param>
        /// <returns></returns>
        public static VectorInt GetVectorInt(int xmin, int xmax, int ymin, int ymax, int zmin, int zmax)
        {
            return new VectorInt()
            {
                x = Range(xmin, xmax),
                y = Range(ymin, ymax),
                z = Range(zmin, zmax),
            };
        }

        /// <summary>
        /// 根据给定的代表 x, y, z 最大值的整数型向量随机取得一个整数型向量
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static VectorInt GetVectorInt(VectorInt max)
        {
            return new VectorInt
            {
                x = Range(0, max.x),
                y = Range(0, max.y),
                z = Range(0, max.z)
            };
        }

        /// <summary>
        /// 根据给定的代表 x, y, z 范围的整数型向量随机取得一个整数型向量
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static VectorInt GetVectorInt(VectorInt min, VectorInt max)
        {
            return  new VectorInt
            {
                x = Range(min.x, max.x),
                y = Range(min.y, max.y),
                z = Range(min.z, max.z)
            };
        }
    }
}
