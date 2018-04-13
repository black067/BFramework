using System;
using System.Collections.Generic;
using BFramework.ExpandedMath;

namespace BFramework.PathFind
{
    [Serializable]
    public class Node
    {
        public Node(int difficulty, int x, int y, int z)
        {
            _property = new Property();
            _property[Property.KEY.DIFFICULTY] = difficulty;
            
            X = x;
            Y = y;
            Z = z;
        }
        
        private Property _property { get; set; }
        
        public Node[,,] Neighbors { get; set; }
        public List<Node> NeighborsI { get; set; }
        public List<Node> NeighborsII { get; set; }
        public List<Node> NeighborsIII { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public bool Opened { get { return _property.Opened; } set { _property.Opened = value; } }
        public bool Closed { get { return _property.Closed; } set { _property.Closed = value; } }
        public Node Parent { get { return _property.Parent; } set { _property.Parent = value; } }
        public int Cost { get { return _property.Cost; } set { _property.Cost = value; } }
        public int Difficulty { get { return _property[Property.KEY.DIFFICULTY]; } set { _property[Property.KEY.DIFFICULTY] = value; } }
        public int GValue { get { return _property[Property.KEY.GVALUE]; } set { _property[Property.KEY.GVALUE] = value; } }
        public int HValue { get { return _property[Property.KEY.HVALUE]; } set { _property[Property.KEY.HVALUE] = value; } }
        public int Resistance { get { return _property[Property.KEY.RESISTANCE]; } set { _property[Property.KEY.RESISTANCE] = value; } }
        public int Temperature { get { return _property[Property.KEY.TEMPERATURE]; } set { _property[Property.KEY.TEMPERATURE] = value; } }
        
        public void SetCost(ref Estimator<Property> estimator)
        {
            _property.Cost = estimator[_property];
        }

        public override string ToString()
        {
            return string.Format("Node(X: {1}, Y: {2}, Z: {3}, Difficulty: {0:D3})", Difficulty, X, Y, Z);
        }
    }
}
