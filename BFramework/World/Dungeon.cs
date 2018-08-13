using System;
using System.Collections.Generic;
using BFramework.PathFind;
using BFramework.ExpandedMath;

namespace BFramework.World
{

    /// <summary>
    /// 地牢生成器
    /// </summary>
    public class Dungeon
    {

        /// <summary>
        /// 地牢房间
        /// </summary>
        public class Room
        {

            /// <summary>
            /// 房间位置
            /// </summary>
            public VectorInt position;

            /// <summary>
            /// 房间的尺寸
            /// </summary>
            public VectorInt size;

            /// <summary>
            /// 房间的边界
            /// </summary>
            public FixedBounds2D bounds;

            /// <summary>
            /// 房间内的节点
            /// </summary>
            public List<Node> nodes;

            /// <summary>
            /// 房间边界上的节点
            /// </summary>
            public List<Node> boundryNodes;

            /// <summary>
            /// 根据位置与尺寸, 构造一个房间, 同时将生成房间的边界信息
            /// </summary>
            /// <param name="position"></param>
            /// <param name="size"></param>
            public Room(VectorInt position, VectorInt size)
            {
                this.position = position;
                this.size = size;
                bounds = new FixedBounds2D(position, size);
            }

            /// <summary>
            /// 判断房间是否与另一个房间由有重叠
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool IsIntersectWith(Room other)
            {
                return bounds.IsIntersectWith(other.bounds);
            }

            /// <summary>
            /// 判断点是否在房间内
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z"></param>
            /// <returns></returns>
            public bool IsPointIn(int x, int y, int z)
            {
                return bounds.IsPointInBounds(new VectorInt(x, y, z));
            }

            /// <summary>
            /// 判断点是否在房间内
            /// </summary>
            /// <param name="point"></param>
            /// <returns></returns>
            public bool IsPointIn(VectorInt point)
            {
                return bounds.IsPointInBounds(point);
            }

            /// <summary>
            /// 判断节点是否在房间内
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            public bool IsNodeIn(Node node)
            {
                return bounds.IsPointInBounds(new VectorInt(node.X, node.Y, node.Z));
            }

            /// <summary>
            /// 对房间进行初始化, 即生成房间内节点列表与边界节点列表
            /// </summary>
            /// <param name="map"></param>
            /// <param name="floorType"></param>
            /// <param name="boundryType"></param>
            /// <param name="minDistanceToMapEdge"></param>
            /// <returns></returns>
            public Room Init(Map map, Properties floorType, Properties boundryType, int minDistanceToMapEdge = 1)
            {
                nodes = nodes ?? new List<Node>();
                nodes.Clear();
                VectorInt diagonal = bounds.Diagonal;
                VectorInt pivot = bounds.pivot;
                int EndI = Math.Min(diagonal.x, Math.Max(0, map.LengthX - 1 - minDistanceToMapEdge)),
                    EndJ = Math.Min(diagonal.y, Math.Max(0, map.LengthY - 1 - minDistanceToMapEdge)),
                    EndK = Math.Min(diagonal.z, Math.Max(0, map.LengthZ - 1 - minDistanceToMapEdge));
                for (int i = pivot.x; i <= EndI; i++)
                {
                    for (int j = pivot.y; j <= EndJ; j++)
                    {
                        for (int k = pivot.z; k <= EndK; k++)
                        {
                            Node n = map[i, j, k];
                            if (n != null)
                            {
                                n.SetProerties(floorType.Clone(true));
                                nodes.Add(n);
                            }
                        }
                    }
                }
                size = new VectorInt(EndI - position.x, EndJ - position.y, EndK - position.z);
                bounds = new FixedBounds2D(position, size);
                boundryNodes = new List<Node>();
                foreach (var point in bounds.boundriesOutside)
                {
                    Node n = map[point.x, point.y, point.z];
                    if (n != null)
                    {
                        n.SetProerties(boundryType);
                        boundryNodes.Add(n);
                    }
                }
                return this;
            }
        }

        /// <summary>
        /// 地牢名字
        /// </summary>
        public string name = "Default";

        /// <summary>
        /// 生成房间时, 最大计算次数, 值越大则房间越密集
        /// </summary>
        public int maxTestTimes = 100;

        /// <summary>
        /// 生成房间的最大尺寸
        /// </summary>
        public VectorInt maxRoomSize = new VectorInt(10, 0, 10);

        /// <summary>
        /// 生成房间的最小尺寸
        /// </summary>
        public VectorInt minRoomSize = new VectorInt(4, 0, 4);

        /// <summary>
        /// 地牢的尺寸
        /// </summary>
        public VectorInt size = new VectorInt(100, 1, 100);

        /// <summary>
        /// 地牢在空间中的原点
        /// </summary>
        public VectorInt origin = VectorInt.Zero;

        /// <summary>
        /// 房间到地牢边界的最小距离
        /// </summary>
        public int minDistanceToEdge = 1;

        /// <summary>
        /// 房间地板的节点类型
        /// </summary>
        public Properties roomFloorPrefab = Default.Properties.Road;

        /// <summary>
        /// 房间边界的节点类型
        /// </summary>
        public Properties boundryNodePrefab = Default.Properties.Rock;

        /// <summary>
        /// 地牢节点的类型
        /// </summary>
        public Properties dungeonNodePrefab = Default.Properties.BaseRock;

        /// <summary>
        /// 生成迷宫的检索代理
        /// </summary>
        public Agent mazeGenerationAgent = Agent.DefaultAgent;
        private Properties empty = Default.Properties.Empty;
        private Properties solid = Default.Properties.Mud;
        private Properties colloid = Default.Properties.Water;

        /// <summary>
        /// 生成地牢的地图信息
        /// </summary>
        public Map generationSpace;

        /// <summary>
        /// 地牢中所有房间
        /// </summary>
        public Room[] rooms;

        /// <summary>
        /// 生成一张地图
        /// </summary>
        /// <returns></returns>
        public Map GenerateSpace()
        {
            return new Map(name, size.x, size.y, size.z, origin, empty);
        }

        /// <summary>
        /// 生成房间
        /// </summary>
        /// <returns></returns>
        public Dungeon GenerateRooms()
        {
            generationSpace = generationSpace ?? GenerateSpace();
            List<Room> roomsList = new List<Room>();
            VectorInt generateStartPoint = new VectorInt(1, 0, 1);
            VectorInt dungeonSizeForGeneration = size - generateStartPoint;
            for (int i = maxTestTimes; i > 0; i--)
            {
                Room r = new Room(BRandom.GetVectorInt(generateStartPoint, dungeonSizeForGeneration),
                    BRandom.GetVectorInt(minRoomSize, maxRoomSize));
                bool isIntersect = false;
                foreach(var room in roomsList)
                {
                    if (room.IsIntersectWith(r))
                    {
                        isIntersect = true;
                        break;
                    }
                }
                if (isIntersect)
                {
                    continue;
                }
                roomsList
                    .Add(r.Init(generationSpace, solid, colloid, minDistanceToEdge));
            }
            rooms = roomsList.ToArray();
            return this;
        }

        /// <summary>
        /// 生成迷宫
        /// </summary>
        /// <returns></returns>
        public Dungeon GenerateMaze()
        {
            string emptytype = Default.Properties.Empty.NodeType;
            Path path;
            Node start, end;
            bool upToDown = BRandom.GetBoolean();
            bool rightToLeft = BRandom.GetBoolean();

            List<Node> boundries = new List<Node>();
            foreach(var r in rooms)
            {
                foreach (var b in r.boundryNodes)
                {
                    boundries.Add(b);
                }
            }

            for(int i = 0, length = maxTestTimes; i < length; i++)
            {
                start = BRandom.GetElement(boundries);
                end = BRandom.GetElement(boundries);
                path = new Path(start, end, mazeGenerationAgent, false);
                path.Find();
                for (int j = 0, lengthJ = maxTestTimes; j < lengthJ && path.State != Path.STATE.SUCCESS; j++)
                {
                    path.Reset();
                    path.Start = BRandom.GetElement(boundries);
                    path.End = BRandom.GetElement(boundries);
                    path.Find();
                }
                foreach (var item in path.Result)
                {
                    if ( item == null ) { continue; }
                    //将节点设置为实心
                    item.SetProerties(solid);

                    if (boundries.Contains(item)) { boundries.Remove(item); }

                    foreach (var neighbor in item.Neighbors)
                    {
                        if (neighbor == null || neighbor.Type == solid.NodeType) { continue; }

                        neighbor.SetProerties(solid);

                        if (boundries.Contains(neighbor)) { boundries.Remove(neighbor); }

                        foreach (var neighborSecondary in neighbor.Neighbors)
                        {
                            if (neighborSecondary == null || neighborSecondary.Type == solid.NodeType) { continue; }

                            neighborSecondary.SetProerties(colloid);

                            if (!boundries.Contains(neighborSecondary)) { boundries.Add(neighborSecondary); }
                        }
                    }
                }
                path.Reset();
            }
            
            foreach (var node in generationSpace.Nodes)
            {
                if (string.Equals(node.Type, empty.NodeType, StringComparison.CurrentCultureIgnoreCase))
                { node.SetProerties(dungeonNodePrefab); }
                if (string.Equals(node.Type, solid.NodeType, StringComparison.CurrentCultureIgnoreCase))
                { node.SetProerties(roomFloorPrefab); }
                if (string.Equals(node.Type, colloid.NodeType, StringComparison.CurrentCultureIgnoreCase))
                { node.SetProerties(boundryNodePrefab); }
            }
            return this;
        }
        
        /// <summary>
        /// 将地牢转换为可读的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string r = base.ToString();
            System.Text.StringBuilder builder = new System.Text.StringBuilder(r);
            builder.Append(string.Format(": {0}\n", name));
            for (int i = generationSpace.LengthZ - 1; i > -1; i--)
            {
                builder.Append(" ");
                for (int j = 0, length = generationSpace.LengthX; j < length; j++)
                {
                    string typeName = generationSpace[j, generationSpace.LengthY - 1, i].Type;
                    string unit = "0";
                    if (string.Equals(typeName, dungeonNodePrefab.NodeType, StringComparison.CurrentCultureIgnoreCase)) { unit = "="; }
                    if (string.Equals(typeName, boundryNodePrefab.NodeType, StringComparison.CurrentCultureIgnoreCase)) { unit = "+"; }
                    if (string.Equals(typeName, roomFloorPrefab.NodeType, StringComparison.CurrentCultureIgnoreCase)) { unit = " "; }
                    if (string.Equals(typeName, empty.NodeType, StringComparison.CurrentCultureIgnoreCase)) { unit = "E"; }
                    if (string.Equals(typeName, colloid.NodeType, StringComparison.CurrentCultureIgnoreCase)) { unit = "C"; }
                    if (string.Equals(typeName, solid.NodeType, StringComparison.CurrentCultureIgnoreCase)) { unit = "S"; }

                    builder.Append(unit);
                }
                builder.Append("\n");
            }

            return builder.ToString();
        }
    }
}
