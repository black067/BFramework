using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BFramework.ExpandedMath;

namespace BFramework.PathFind
{
    public class Block
    {
        public struct Mark
        {
            public Mark(Block father, int gValue, int hValue, bool closed = false) : this()
            {
                Closed = closed;
                Father = father;
                GValue = gValue;
                HValue = hValue;
                Cost = GValue + HValue;
            }

            public bool Closed { get; set; }
            public Block Father { get; set; }
            public int GValue { get; set; }
            public int HValue { get; set; }
            public int Cost { get; set; }

            public void Set(Block father, int gValue, int hValue, bool closed = false)
            {
                Closed = closed;
                Father = father;
                GValue = gValue;
                HValue = hValue;
                Cost = GValue + HValue;
            }
        }

        public Block(int weight, int x, int y, int z)
        {
            Weight = weight;
            X = x;
            Y = y;
            Z = z;
            tag = new Mark(null, int.MaxValue, int.MaxValue);
        }

        public Mark tag;

        public int Weight { get; set; }
        public Block[,,] Neighbor { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override string ToString()
        {
            return string.Format("Block(Weight: {0}, X: {1}, Y: {2}, Z: {3})", Weight, X, Y, Z);
        }
    }
}
