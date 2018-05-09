using BFramework.World;

namespace BFramework.PathFind
{
    public class Heuristic
    {
        public enum TYPE
        {
            MANHATTAN,
            EUCLIDEAN,
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

        public int Calculate(Node start, Node target)
        {
            switch (Type)
            {
                case TYPE.MANHATTAN:
                    return Manhattan(start, target);
                case TYPE.EUCLIDEAN:
                    return Euclidean(start, target);
            }
            return int.MaxValue;
        }

    }
}