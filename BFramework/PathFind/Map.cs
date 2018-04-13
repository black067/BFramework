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

            SetNeighbors();
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

        public void SetNeighbors(Node node)
        {
            int x = node.X, y = node.Y, z = node.Z;
            Node[,,] neighbors = new Node[3, 3, 3];
            List<Node> neighborsI = new List<Node>();
            List<Node> neighborsII = new List<Node>();
            List<Node> neighborsIII = new List<Node>();
            Node current;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        if (Check(x + i, y + j, z + k))
                        {
                            current = Nodes[x + i, y + j, z + k];
                            neighbors[i + 1, j + 1, k + 1] = current;
                            int djk = j - k;
                            if (i != 0 && j != 0 && k != 0)
                            {
                                neighborsIII.Add(current);
                            }
                            else if ((i == 1 && (djk == 1 || djk == -1)) || (i != 1 && djk == 0))
                            {
                                neighborsI.Add(current);
                            }
                            else
                            {
                                if (i == 1 && j == 1 && k == 1) continue;
                                neighborsII.Add(current);
                            }
                        }
                    }
                }
            }
            node.Neighbors = neighbors;
            node.NeighborsI = neighborsI;
            node.NeighborsII = neighborsII;
            node.NeighborsIII = neighborsIII;
        }

        public void SetNeighbors(Node[,,] nodes)
        {
            foreach (Node node in nodes)
            {
                SetNeighbors(node);
            }
        }

        public void SetNeighbors()
        {
            foreach (Node node in Nodes)
            {
                SetNeighbors(node);
            }
        }

        public bool Check(int x, int y, int z) { return x >= 0 && x < LengthX && y >= 0 && y < LengthY && z >= 0 && z < LengthZ; }

        public void SetNode(int x, int y, int z, int difficulty)
        {
            if (Check(x,y,z))
            {
                Nodes[x, y, z].Difficulty = difficulty;
                Nodes[x, y, z].Resistance = difficulty > 0 ? 1 : 0;
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
