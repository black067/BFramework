using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BFramework.ExpandedMath;

namespace BFramework.PathFind
{
    public class Node
    {
        public struct Mark
        {
            public Mark(Node father, int gValue, int hValue, bool closed = false) : this()
            {
                Closed = closed;
                Father = father;
                GValue = gValue;
                HValue = hValue;
                Cost = GValue + HValue;
            }

            public bool Closed { get; set; }
            public Node Father { get; set; }
            public int GValue { get; set; }
            public int HValue { get; set; }
            public int Cost { get; set; }

            public void Set(Node father, int gValue, int hValue, int cost, bool closed = false)
            {
                Closed = closed;
                Father = father;
                Cost = cost;
            }
        }

        public class Attribute : IEstimable<int>
        {
            private static int _count = 5;
            public int Weight { get; set; }
            public int GValue { get; set; }
            public int HValue { get; set; }
            public int Resistance { get; set; }
            public int Temperature { get; set; }

            public Attribute(Attribute origin)
            {
                Weight = origin.Weight;
                GValue = origin.GValue;
                HValue = origin.HValue;
                Resistance = origin.Resistance;
                Temperature = origin.Temperature;
            }

            public Attribute()
            {
                Weight = 0;
                GValue = int.MaxValue;
                HValue = int.MaxValue;
                Resistance = 0;
                Temperature = 0;
            }

            public void Add(Attribute addition)
            {
                Weight += addition.Weight;
                GValue += addition.GValue;
                HValue += addition.HValue;
                Resistance += addition.Resistance;
                Temperature += addition.Temperature;
            }

            public void Multiply(Attribute multiplier)
            {
                Weight *= multiplier.Weight;
                GValue *= multiplier.GValue;
                HValue *= multiplier.HValue;
                Resistance *= multiplier.Resistance;
                Temperature *= multiplier.Temperature;
            }


            public void Add(IEstimable<int> addition)
            {
                Add((Attribute)addition);
            }

            public void Add(int addition)
            {
                Weight += addition;
                GValue += addition;
                HValue += addition;
                Resistance += addition;
                Temperature += addition;
            }

            public IEstimable<int> Clone()
            {
                return new Attribute(this);
            }

            public int GetCount()
            {
                return _count;
            }

            public void Multiply(IEstimable<int> multiplier)
            {
                Multiply((Attribute)multiplier);
            }

            public void Multiply(int multiplier)
            {
                Weight *= multiplier;
                GValue *= multiplier;
                HValue *= multiplier;
                Resistance *= multiplier;
                Temperature *= multiplier;
            }

            public int Sum()
            {
                return Weight + GValue + HValue + Resistance + Temperature;
            }
        }

        public Node(int weight, int x, int y, int z)
        {
            Weight = weight;
            X = x;
            Y = y;
            Z = z;
            tag = new Mark(null, int.MaxValue, int.MaxValue);
        }

        public Mark tag;
        public Attribute property;

        public int Weight { get; set; }
        public Node[,,] Neighbor { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override string ToString()
        {
            return string.Format("Block(Weight: {0:D3}, X: {1}, Y: {2}, Z: {3})", Weight, X, Y, Z);
        }
    }
}
