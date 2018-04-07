using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BFramework.ExpandedMath;

namespace BFramework.PathFind
{
    public class Block
    {
        public Block(int iD, Block[] neighbor, Vector location, int power, float distanceToStart = float.PositiveInfinity, float distanceToEnd = float.PositiveInfinity)
        {
            ID = iD;
            Power = power;
            DistanceToStart = distanceToStart;
            DistanceToEnd = distanceToEnd;
            Cost = float.PositiveInfinity;
            Neighbor = neighbor;
            Location = location;
        }

        public int ID
        {
            get; set;
        }

        public int Power
        {
            get; set;
        }
        private float DistanceToStart
        {
            get; set;
        }
        private float DistanceToEnd
        {
            get; set;
        }
        public float Cost
        {
            get; set;
        }
        public Block[] Neighbor
        {
            get; set;
        }
        public Vector Location
        {
            get; set;
        }
        
    }
}
