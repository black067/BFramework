using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.PathFind
{
    [Serializable]
    public class Map
    {
        public Map(string name, int lengthX, int lengthY, int lengthZ, bool randomWeight = false)
        {
            Name = name;
            LengthX = lengthX;
            LengthY = lengthY;
            LengthZ = lengthZ;
            NodesNumber = LengthX * LengthY * LengthZ;
            Nodes = new Node[LengthX, LengthY, LengthZ];
            for (int i = 0; i < LengthX; i++)
            {
                for (int j = 0; j < LengthY; j++)
                {
                    for (int k = 0; k < LengthZ; k++)
                    {
                        Nodes[i, j, k] = new Node(randomWeight ? ExpandedMath.Random.Range(0, 999) : 0, i, j, k);
                    }
                }
            }
            foreach (Node node in Nodes)
            {
                node.Neighbors = GetNeighbors(node, false);
                node.NeighborsNarrow = GetNeighbors(node, true);
            }
        }

        public string Name { get; set; }
        public int LengthX { get; set; }
        public int LengthY { get; set; }
        public int LengthZ { get; set; }
        public int NodesNumber { get; set; }
        public Node[,,] Nodes { get; set; }
        
        public Node this[int x, int y, int z]
        {
            get
            {
                return Check(x, y, z) ? Nodes[x, y, z] : null;
            }
        }

        public List<Node> GetNeighbors(int x, int y, int z, bool narrowlyDefined = false)
        {
            List<Node> neightbors = new List<Node>(26);
            if (!narrowlyDefined)
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        for (int k = -1; k < 2; k++)
                        {
                            if (Check(x + i, y + j, z + k))
                            {
                                neightbors.Add(Nodes[x + i, y + j, z + k]);
                            }
                        }
                    }
                }
            }
            else
            {
                if (Check(x + 1, y, z)) neightbors.Add(Nodes[x + 1, y, z]);
                if (Check(x - 1, y, z)) neightbors.Add(Nodes[x - 1, y, z]);
                if (Check(x, y + 1, z)) neightbors.Add(Nodes[x, y + 1, z]);
                if (Check(x, y - 1, z)) neightbors.Add(Nodes[x, y - 1, z]);
                if (Check(x, y, z + 1)) neightbors.Add(Nodes[x, y, z + 1]);
                if (Check(x, y, z - 1)) neightbors.Add(Nodes[x, y, z - 1]);
            }
            return neightbors;
        }

        public List<Node> GetNeighbors(Node node, bool narrowlyDefined = false)
        {
            return GetNeighbors(node.X, node.Y, node.Z, narrowlyDefined);
        }

        public bool Check(int x, int y, int z) { return x >= 0 && x < LengthX && y >= 0 && y < LengthY && z >= 0 && z < LengthZ; }

        public void SetNode(int x, int y, int z, int difficulty)
        {
            if (Check(x,y,z))
            {
                Nodes[x, y, z].Difficulty = difficulty;
            }
        }

        public void SetNode(Node node, int difficulty)
        {
            SetNode(node.X, node.Y, node.Z, difficulty);
        }

        public override string ToString()
        {
            return string.Format("Map(Name: {0}, LengthX: {1}, LengthY: {2}, LengthZ: {3})", Name, LengthX, LengthY, LengthZ);
        }
    }
}
