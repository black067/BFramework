using System;
using BFramework.World;

namespace BFramework.PathFind
{
    /// <summary>
    /// 启发函数
    /// </summary>
    public class Heuristic
    {
        /// <summary>
        /// 启发函数类型枚举
        /// </summary>
        public enum TYPE
        {
            MANHATTAN,
            EUCLIDEAN,
            EUCLIDEANSQUARE,
            OCTILE,
            CHEBYSHEV,
        }
        
        /// <summary>
        /// 快速求绝对值
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static int Abs(int n)
        {
            return n > 0 ? n : -n;
        }

        /// <summary>
        /// 查表法计算节点与邻节点的距离
        /// </summary>
        /// <param name="node"></param>
        /// <param name="neighbor"></param>
        /// <returns></returns>
        public static double ComputeNeighborDistance(Node node, Node neighbor)
        {
            int b = (node.X == neighbor.X ? 1 : 0) + (node.Y == neighbor.Y ? 2 : 0) + (node.Z == neighbor.Z ? 4 : 0);
            return (b == 3 || b == 5 || b == 6) ? 1 : sqrt2;
        }

        /// <summary>
        /// 将三个整型数排序
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int[] Sort3(int a, int b, int c)
        {
            bool aBTb = a > b, aBTc = a > c, bBTc = b > c;
            // a > b, b > c
            // => a, b, c
            if (aBTb && bBTc) { return new int[] { a, b, c }; }
            // a > c, c >= b
            // => a, c, b
            if (aBTc && !bBTc) { return new int[] { a, c, b }; }
            // b >= a, a > c
            // => b, a, c
            if (!aBTb && aBTc) { return new int[] { b, a, c }; }
            // b > c, c >= a
            // => b, c, a
            if (bBTc && !aBTc) { return new int[] { b, c, a }; }
            // c >= a, a > b
            // => c, a, b
            if (!aBTc && aBTb) { return new int[] { c, a, b }; }
            // c >= b, b >= a
            // => c, b, a
            if (!bBTc && !aBTb) { return new int[] { c, b, a }; }

            return new int[3] { a, b, c };
        }

        /// <summary>
        /// 求两个节点间的曼哈顿距离
        /// </summary>
        /// <param name="start"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int Manhattan(Node start, Node target)
        {
            if (target == null)
            {
                return 0;
            }
            return Abs(target.X - start.X) + Abs(target.Y - start.Y) + Abs(target.Z - start.Z);
        }

        /// <summary>
        /// 求两个节点间的欧几里得距离, 但不开根号
        /// </summary>
        /// <param name="start"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double EuclideanSquare(Node start, Node target)
        {
            if (target == null)
            {
                return 0;
            }
            int dX = target.X - start.X;
            int dY = target.Y - start.Y;
            int dZ = target.Z - start.Z;
            return dX * dX + dY * dY + dZ * dZ;
        }
        
        public static readonly double sqrt2 = Math.Sqrt(2);
        
        public static readonly double sqrt3 = Math.Sqrt(3);

        /// <summary>
        /// 求两个节点间的 Octile 距离
        /// </summary>
        /// <param name="start"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double Octile(Node start, Node target)
        {
            if (target == null) { return 0; }
            int[] sorted = Sort3(Abs(start.X - target.X), Abs(start.Y - target.Y), Abs(start.Z - target.Z));
            int max = sorted[0], mid = sorted[1], min = sorted[2];
            double dist = sqrt3 * min;
            max -= min;
            mid -= min;
            dist += sqrt2 * min;
            max -= min;
            dist += max;
            return dist;
        }

        /// <summary>
        /// 求两个节点间的切比雪夫距离
        /// </summary>
        /// <param name="start"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double Chebyshev(Node start, Node target)
        {
            if (target == null)
            {
                return 0;
            }
            int[] sorted = Sort3(Abs(start.X - target.X), Abs(start.Y - target.Y), Abs(start.Z - target.Z));
            return sorted[0];
        }

        /// <summary>
        /// 根据输入的类型与节点选择相应的启发函数计算节点间的距离
        /// </summary>
        /// <param name="start"></param>
        /// <param name="target"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static double Calculate(Node start, Node target, TYPE type)
        {
            switch (type)
            {
                case TYPE.MANHATTAN:
                    return Manhattan(start, target);
                case TYPE.EUCLIDEAN:
                    return Math.Sqrt(EuclideanSquare(start, target));
                case TYPE.EUCLIDEANSQUARE:
                    return EuclideanSquare(start, target);
                case TYPE.OCTILE:
                    return Octile(start, target);
                case TYPE.CHEBYSHEV:
                    return Chebyshev(start, target);
            }
            return double.MaxValue;
        }

    }
}