using System;
using BFramework.World;

namespace BFramework.PathFind
{
    public class Heuristic
    {
        public enum TYPE
        {
            MANHATTAN,
            EUCLIDEAN,
            OCTILE,
        }

        public Heuristic(TYPE type = TYPE.EUCLIDEAN)
        {
            Type = type;
        }

        public TYPE Type { get; set; }

        private static int Abs(int n)
        {
            return n > 0 ? n : -n;
        }

        private static int Max(int a, int b)
        {
            return a > b ? a : b;
        }

        private static int Min(int a, int b)
        {
            return a > b ? b : a;
        }

        private static int Min(params int[] args)
        {
            int r = int.MaxValue;
            for(int i = args.Length -1;i > -1; i--)
            {
                r = args[i] < r ? args[i] : r;
            }
            return r;
        }

        private static int Max(params int[] args)
        {
            int r = int.MinValue;
            for(int i = args.Length -1;i > -1; i--)
            {
                r = args[i] > r ? args[i] : r;
            }
            return r;
        }

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

        public static int Manhattan(Node start, Node target)
        {
            if (target == null)
            {
                return int.MaxValue;
            }
            return Abs(target.X - start.X) + Abs(target.Y - start.Y) + Abs(target.Z - start.Z);
        }

        public static int Euclidean(Node start, Node target)
        {
            if (target == null)
            {
                return int.MaxValue;
            }
            int dX = target.X - start.X;
            int dY = target.Y - start.Y;
            int dZ = target.Z - start.Z;
            return dX * dX + dY * dY + dZ * dZ;
        }

        public static readonly double sqrt2 = Math.Sqrt(2);

        public static readonly double sqrt3 = Math.Sqrt(3);

        public static double Octile(Node start, Node target)
        {
            if (target == null) { return int.MaxValue; }
            int[] sorted = Sort3(Abs(start.X - target.Y), Abs(start.Y - target.Y), Abs(start.Z - target.Z));
            int max = sorted[0], mid = sorted[1], min = sorted[2];
            double dist = 0;
            dist += sqrt3 * min;
            max -= min;
            mid -= min;
            dist += sqrt2 * min;
            max -= min;
            dist += max;
            return dist;
        }

        public static double Calculate(Node start, Node target, TYPE type)
        {
            switch (type)
            {
                case TYPE.MANHATTAN:
                    return Manhattan(start, target);
                case TYPE.EUCLIDEAN:
                    return Euclidean(start, target);
                case TYPE.OCTILE:
                    return Octile(start, target);
            }
            return double.MaxValue;
        }

    }
}