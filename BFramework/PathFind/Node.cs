using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BFramework.ExpandedMath;

namespace BFramework.PathFind
{
    public class Node
    {
        public class Attribute : IEstimable<int>
        {
            private bool _closed;
            private bool _opened;

            public Node Parent { get; set; }
            public bool Closed
            {
                get { return _closed; }
                set { _closed = value; }
            }
            public bool Opened
            {
                get { return _opened; }
                set { _opened = value; }
            }
            public int Cost { get; set; }

            private static int _count = 5;
            public int Difficulty { get; set; }
            public int GValue { get; set; }
            public int HValue { get; set; }
            public int Resistance { get; set; }
            public int Temperature { get; set; }

            public Attribute(Attribute origin)
            {
                Difficulty = origin.Difficulty;
                GValue = origin.GValue;
                HValue = origin.HValue;
                Resistance = origin.Resistance;
                Temperature = origin.Temperature;
            }

            public Attribute()
            {
                Difficulty = 0;
                GValue = 1;
                HValue = 1;
                Resistance = 0;
                Temperature = 0;
            }

            public void Add(Attribute addition)
            {
                Difficulty += addition.Difficulty;
                GValue += addition.GValue;
                HValue += addition.HValue;
                Resistance += addition.Resistance;
                Temperature += addition.Temperature;
            }

            public void Multiply(Attribute multiplier)
            {
                Difficulty *= multiplier.Difficulty;
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
                Difficulty += addition;
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
                Difficulty *= multiplier;
                GValue *= multiplier;
                HValue *= multiplier;
                Resistance *= multiplier;
                Temperature *= multiplier;
            }

            public int Sum()
            {
                return Difficulty + GValue + HValue + Resistance + Temperature;
            }
        }

        public Node(int weight, int x, int y, int z)
        {
            property = new Attribute() { Difficulty = weight };
            X = x;
            Y = y;
            Z = z;
        }
        
        public Attribute property;
        
        public List<Node> Neighbor { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public int Difficulty { get { return property.Difficulty; } }
        public Node Parent { get { return property.Parent; } set { property.Parent = value; } }

        public void SetCost(ref Estimator<Attribute> estimator)
        {
            property.Cost = estimator[property];
        }

        public override string ToString()
        {
            return string.Format("Node(Difficulty: {0:D3}, X: {1}, Y: {2}, Z: {3})", Difficulty, X, Y, Z);
        }
    }
}
