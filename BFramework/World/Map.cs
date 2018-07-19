using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BFramework.World
{
    /// <summary>
    /// 地图类, 保存地形
    /// </summary>
    [System.Serializable]
    public class Map : ISerializable
    {
        public static readonly string Extension = ".map";

        /// <summary>
        /// 根据给定长宽高新建一个地图, 可选择是否随机给节点的通行难度赋值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lengthX"></param>
        /// <param name="lengthY"></param>
        /// <param name="lengthZ"></param>
        /// <param name="randomDifficulty"></param>
        public Map(string name, int lengthX, int lengthY, int lengthZ, bool randomDifficulty = false)
        {
            Name = name;
            LengthX = lengthX;
            LengthY = lengthY;
            LengthZ = lengthZ;
            NodesCount = LengthX * LengthY * LengthZ;
            Nodes = new Node[LengthX, LengthY, LengthZ];
            for (int i = 0; i < LengthX; i++)
            {
                for (int j = 0; j < LengthY; j++)
                {
                    for (int k = 0; k < LengthZ; k++)
                    {
                        Nodes[i, j, k] = new Node(i, j, k, randomDifficulty ? Default.Properties.Random : Default.Properties.Empty);
                    }
                }
            }
            SetNeighbors();
        }

        /// <summary>
        /// 地图名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 地图在 X 轴方向的长度
        /// </summary>
        public int LengthX { get; set; }

        /// <summary>
        /// 地图在 Y 轴方向的长度
        /// </summary>
        public int LengthY { get; set; }

        /// <summary>
        /// 地图在 Z 轴方向的长度
        /// </summary>
        public int LengthZ { get; set; }

        /// <summary>
        /// 地图中的节点总量
        /// </summary>
        public int NodesCount { get; set; }

        /// <summary>
        /// 地图中的所有节点
        /// </summary>
        public Node[,,] Nodes { get; set; }

        public List<string> PrefabNodeTypes { get; set; }

        public int[,,] NodeTypes { get; set; }

        /// <summary>
        /// 节点访问器, 根据坐标返回对应的节点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Node this[int x, int y, int z]
        {
            get
            {
                return Check(x, y, z) ? Nodes[x, y, z] : null;
            }
        }

        /// <summary>
        /// 设定节点的相邻节点
        /// </summary>
        /// <param name="node"></param>
        public void SetNeighbors(Node node)
        {
            int x = node.X, y = node.Y, z = node.Z;
            Node[,,] neighbors = new Node[3, 3, 3];
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
                        }
                    }
                }
            }
            node.Neighbors = neighbors;
        }
        /// <summary>
        /// 设置自身所有节点的相邻节点
        /// </summary>
        public void SetNeighbors()
        {
            foreach (Node node in Nodes)
            {
                SetNeighbors(node);
            }
        }

        /// <summary>
        /// 根据坐标检查该坐标是否在地图内
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public bool Check(int x, int y, int z) { return x >= 0 && x < LengthX && y >= 0 && y < LengthY && z >= 0 && z < LengthZ; }

        public static void SetNode(Node node, Properties newProperties)
        {
            if (newProperties == null)
            {
                newProperties = new Properties();
            }
            node.SetProerties(newProperties);
        }

        /// <summary>
        /// 地图类转化为字符串的方法重载
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Map(Name: {0}, LengthX: {1}, LengthY: {2}, LengthZ: {3})", Name, LengthX, LengthY, LengthZ);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("LengthX", LengthX);
            info.AddValue("LengthY", LengthY);
            info.AddValue("LengthZ", LengthZ);
            PrefabNodeTypes = new List<string>() { Default.Properties.Empty.NodeType};
            NodeTypes = new int[LengthX, LengthY, LengthZ];
            for (int i = 0; i < LengthX; i++)
            {
                for (int j = 0; j < LengthY; j++)
                {
                    for (int k = 0; k < LengthZ; k++)
                    {
                        string typeName = Nodes[i, j, k].Type;
                        if (!PrefabNodeTypes.Contains(typeName))
                        {
                            PrefabNodeTypes.Add(typeName);
                        }
                        NodeTypes[i, j, k] = PrefabNodeTypes.IndexOf(typeName);
                    }
                }
            }
            info.AddValue("PrefabNodeTypes", PrefabNodeTypes);
            info.AddValue("NodeTypes", NodeTypes);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected Map(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("Name");
            LengthX = info.GetInt32("LengthX");
            LengthY = info.GetInt32("LengthY");
            LengthZ = info.GetInt32("LengthZ");
            PrefabNodeTypes = (List<string>)info.GetValue("PrefabNodeTypes", typeof(List<string>));
            System.Console.WriteLine(PrefabNodeTypes.Count);
            NodeTypes = (int[,,])info.GetValue("NodeTypes", typeof(int[,,]));
            Nodes = new Node[LengthX, LengthY, LengthZ];
            for (int i = 0; i < LengthX; i++)
            {
                for (int j = 0; j < LengthY; j++)
                {
                    for (int k = 0; k < LengthZ; k++)
                    {
                        Nodes[i, j, k] = new Node(i, j, k, new Properties(PrefabNodeTypes[NodeTypes[i, j, k]]));
                    }
                }
            }
            SetNeighbors();
        }
    }
}
