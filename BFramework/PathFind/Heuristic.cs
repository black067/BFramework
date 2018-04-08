using System;
using System.Collections.Generic;

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

        private int Abs(int n)
        {
            return n > 0 ? n : -n;
        }

        public int Manhattan(Block start, Block target)
        {
            if (target == null)
            {
                return 0;
            }
            return Abs(target.X - start.X) + Abs(target.Y - start.Y) + Abs(target.Z - start.Z);
        }

        public int Euclidean(Block start, Block target)
        {
            if (target == null) { return 0; }
            int dX = target.X - start.X;
            int dY = target.Y - start.Y;
            int dZ = target.Z - start.Z;
            return dX * dX + dY * dY + dZ * dZ;
        }

        public int Calculate(Block start, Block target)
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