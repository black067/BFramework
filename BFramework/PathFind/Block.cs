using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BFramework.ExpandedMath;

namespace BFramework.PathFind
{
    public class Block
    {
        public Block(int weight, int x, int y, int z)
        {
            Weight = weight;
            X = x;
            Y = y;
            Z = z;
        }

        public Map Map { get; set; }
        public int Weight { get; set; }
        public Block[,,] Neighbor { get; set; }
        public Vector Location { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override string ToString()
        {
            return string.Format("Block(Weight: {0}, X: {1}, Y: {2}, Z: {3})", Weight, X, Y, Z);
        }
    }
}
