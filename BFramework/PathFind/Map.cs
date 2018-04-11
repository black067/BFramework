using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.PathFind
{
    public class Map
    {
        public Map(string name, int lengthX, int lengthY, int lengthZ, bool randomWeight = false)
        {
            Name = name;
            LengthX = lengthX;
            LengthY = lengthY;
            LengthZ = lengthZ;
            BlockNumber = LengthX * LengthY * LengthZ;
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

            foreach (Node node in Nodes) {
                node.Neighbor = GetNeighbor(node.X, node.Y, node.Z);
            }
        }

        public string Name { get; set; }
        public int LengthX { get; set; }
        public int LengthY { get; set; }
        public int LengthZ { get; set; }
        public int BlockNumber { get; set; }
        public Node[,,] Nodes { get; set; }
        
        public Node this[int x, int y, int z]
        {
            get
            {
                return Check(x, y, z) ? Nodes[x, y, z] : null;
            }
        }

        public Node[,,] GetNeighbor(int x, int y, int z)
        {
            Node[,,] neightbor = new Node[3,3,3];
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        neightbor[i + 1, j + 1, k + 1] = Check(x + i, y + j, z + k) ? Nodes[x + i, y + j, z + k] : null;
                    }
                }
            }
            return neightbor;
        }

        public bool Check(int x, int y, int z) { return x >= 0 && x < LengthX && y >= 0 && y < LengthY && z >= 0 && z < LengthZ; }

        public void SetBlock(int difficulty, int x, int y, int z)
        {
            if (Check(x,y,z))
            {
                Nodes[x, y, z].property.Difficulty = difficulty;
            }
        }

        public override string ToString()
        {
            return string.Format("Map(Name: {0}, LengthX: {1}, LengthY: {2}, LengthZ: {3})", Name, LengthX, LengthY, LengthZ);
        }
    }
}
